from flask import Flask
from flask_restful import Resource, Api, reqparse

app = Flask(__name__)
api = Api(app)

class Items(Resource):
    def post(self):
        parser = reqparse.RequestParser()  # initialize
        
        parser.add_argument('family', required=True)  # add args
        parser.add_argument('passphrase', required=True)
        parser.add_argument('given', required=True)
        
        args = parser.parse_args()  # parse arguments to dictionary
        
        # # create new dataframe containing new values
        new_data = {
            'family': args['family'],
            'given': args['given'],
            'passphrase': args['passphrase']
        }
        # read our CSV
        # data = pd.read_csv('users.csv')
        # # add the newly provided values
        # data = data.append(new_data, ignore_index=True)
        # # save back to CSV
        # data.to_csv('users.csv', index=False)
        return {'data': new_data}, 200  # return data with 200 OK
    
    def get(self):
        print("get")
        return {'hello': 'hello'}, 200

api.add_resource(Items, '/items')  # '/users' is our entry point

if __name__ == '__main__':
    app.run(host='0.0.0.0')  # run our Flask app