from flask import Flask
from flask_restful import Resource, Api, reqparse
import mysql.connector
import time
import sys

app = Flask(__name__)
api = Api(app)

db = mysql.connector.connect(
    host="db",
    user="admin",
    password="toor",
    port=3306,
    database="artichoke",
    autocommit=True # without this the api will only update locally and things act funny
)

cursor = db.cursor()
cursor.execute("SELECT sub_expires_on FROM families WHERE family_name='Shapiro' AND passphrase_hash='sha256'")

for x in cursor:
    print(time.ctime(x[0]))
    if time.time() < int(x[0]):
        print('valid')

class MyFamily(Resource):
    def post(self):
        parser = reqparse.RequestParser()  # initialize
        
        parser.add_argument('family_id', required=True)  # add args
        parser.add_argument('passphrase_hash', required=True)
        parser.add_argument('given_id', required=True)
        
        args = parser.parse_args()  # parse arguments to dictionary

        cursor.execute("SELECT * FROM families WHERE id=%s AND passphrase_hash=%s", ( args['family_id'], args['passphrase_hash'] ))
        
        row = []
        for x in cursor:
            row = x

        # # create new dataframe containing new values
        data = {
            'family': row[1],
            'expires_on': row[3],
            'members': []
        }

        cursor.execute("SELECT * FROM users WHERE family_id=%s", (row[0], ))
        for x in cursor:
            member = {
                'given': x[1]
            }
            data['members'].append(member)
        

        return data, 200  # return data with 200 OK

class ItemsLeft(Resource):
    def post(self):
        parser = reqparse.RequestParser()  # initialize
        
        parser.add_argument('family_id', required=True)  # add args
        parser.add_argument('passphrase_hash', required=True)
        parser.add_argument('given_id', required=True)
        
        args = parser.parse_args()  # parse arguments to dictionary

        cursor.execute("SELECT id FROM families WHERE id=%s AND passphrase_hash=%s", ( args['family_id'], args['passphrase_hash'] ))

        family_ids = []
        for row in cursor:
            family_ids.append(row[0])
        
        if len(family_ids) != 1:
            return {}, 401

        data = {
            'items': []
        }

        cursor.execute("SELECT items.id, item, c.category FROM items INNER JOIN categories c on items.category_id = c.id WHERE items.family_id=%s AND collected=0 ORDER BY c.category", (family_ids[0], ))
        
        for row in cursor:
            print(row, file=sys.stderr)
            item = {
                'id': row[0],
                'item': row[1],
                'category': row[2]
            }
            data['items'].append(item)
        
        # read our CSV
        # data = pd.read_csv('users.csv')
        # # add the newly provided values
        # data = data.append(new_data, ignore_index=True)
        # # save back to CSV
        # data.to_csv('users.csv', index=False)
        return data['items'], 200  # return data with 200 OK
    
    def get(self):
        print("get")
        return {'hello': 'hello'}, 200

class ItemsCollected(Resource):
    def post(self):
        parser = reqparse.RequestParser()  # initialize
        
        parser.add_argument('family_id', required=True)  # add args
        parser.add_argument('passphrase_hash', required=True)
        parser.add_argument('given_id', required=True)
        
        args = parser.parse_args()  # parse arguments to dictionary

        cursor.execute("SELECT id FROM families WHERE id=%s AND passphrase_hash=%s", ( args['family_id'], args['passphrase_hash'] ))

        family_ids = []
        for row in cursor:
            family_ids.append(row[0])
        
        if len(family_ids) != 1:
            return {}, 401

        data = {
            'items': []
        }

        cursor.execute("SELECT items.id, item, c.category FROM items INNER JOIN categories c on items.category_id = c.id WHERE items.family_id=%s AND collected=1 ORDER BY modified_on DESC LIMIT 10", (family_ids[0], ))

        for row in cursor:
            item = {
                'id': row[0],
                'item': row[1],
                'category': row[2]
            }
            data['items'].append(item)
        
        # read our CSV
        # data = pd.read_csv('users.csv')
        # # add the newly provided values
        # data = data.append(new_data, ignore_index=True)
        # # save back to CSV
        # data.to_csv('users.csv', index=False)
        return data['items'], 200  # return data with 200 OK
    
    def get(self):
        print("get")
        return {'hello': 'hello'}, 200

class ItemCollect(Resource):
    def post(self):
        parser = reqparse.RequestParser()  # initialize
        
        parser.add_argument('family_id', required=True)  # add args
        parser.add_argument('passphrase_hash', required=True)
        parser.add_argument('given_id', required=True)
        parser.add_argument('item_id', required=True)
        
        args = parser.parse_args()  # parse arguments to dictionary

        cursor.execute("SELECT id FROM families WHERE id=%s AND passphrase_hash=%s", ( args['family_id'], args['passphrase_hash'] ))

        family_ids = []
        for row in cursor:
            family_ids.append(row[0])
        
        if len(family_ids) != 1:
            return {}, 401

        data = {
            'message': 'success'
        }

        cursor.execute("CALL item_collect(%s, %s, %s)", (family_ids[0], args['given_id'], args['item_id']))
        for row in cursor:
            print(row, file=sys.stderr)
        # db.commit() # without this the api will only update locally and things act funny
        
        return data, 200

class GetCategories(Resource):
    def post(self):
        parser = reqparse.RequestParser()  # initialize
        
        parser.add_argument('family_id', required=True)  # add args
        parser.add_argument('passphrase_hash', required=True)
        parser.add_argument('given_id', required=True)
        parser.add_argument('item_id', required=True)
        
        args = parser.parse_args()  # parse arguments to dictionary

        cursor.execute("SELECT id FROM families WHERE id=%s AND passphrase_hash=%s", ( args['family_id'], args['passphrase_hash'] ))

        family_ids = []
        for row in cursor:
            family_ids.append(row[0])
        
        if len(family_ids) != 1:
            return {}, 401

        data = {
            'categories' : []
        }

        cursor.execute("CALL get_categories(%s)", (family_ids[0], ))
        for row in cursor:
            print(row, file=sys.stderr)
            category = {
                'id' : row[0],
                'name' : row[1]
            }
            data['categories'].append(category)
        
        return data['categories'], 200

api.add_resource(ItemsLeft, '/itemsleft')  # '/users' is our entry point
api.add_resource(ItemsCollected, '/itemscollected')
api.add_resource(MyFamily, '/myfamily')
api.add_resource(GetCategories, '/getcategories')

api.add_resource(ItemCollect, '/itemcollect')

if __name__ == '__main__':
    app.run(host='0.0.0.0')  # run our Flask app