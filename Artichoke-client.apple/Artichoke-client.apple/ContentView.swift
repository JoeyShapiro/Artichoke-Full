//
//  ContentView.swift
//  Artichoke-client.apple
//
//  Created by Joey Shapiro on 9/3/22.
//

import SwiftUI

struct Item {
    var id: UUID
    var category: String
    var name: String
    var collected: Bool
}

struct ContentView: View {
    // MARK: - View State
    @State var showingSheet = false
    @State var items = [Item]()
    @State var itemName = ""
    @State var itemCategory = "Deli"
    var body: some View {
        GeometryReader { geometry in
            List {
                HStack {
                    Picker("Category", selection: $itemCategory) {
                        Text("Deli").tag("Deli")
                        Text("Bread").tag("Bread")
                        Text("Frozen").tag("Frozen")
                        Text("Canned").tag("Canned")
                    }.pickerStyle(MenuPickerStyle())
                        .frame(width: geometry.size.width * 0.1)
                        .contentShape(Rectangle())
                    
                    HStack {
                        TextField("Item", text: $itemName)
                        
                        if self.itemName.count > 0 {
                            Button(action: {
                                AddItem(items: &self.items, name: itemName, category: itemCategory)
                                self.itemName = ""
                                self.itemCategory = "Deli"
//                                makePostCall()
                                apiCall()
                            }) {
                                Image(systemName: "plus.circle.fill")
                                    .foregroundColor(Color.green).imageScale(.large)
                            }
                        }
                    }
                }
                Section(header: Text("In Progress")) {
                    ForEach(items, id: \.id) { item in
                        if !item.collected {
                            Button(action: {
                                self.showingSheet = true
                            }) {
                                HStack {
                                    Text(item.category)
                                        .frame(width: geometry.size.width * 0.1)
                                    Text(item.name)
                                }
                            }
                        }
                    }.onDelete(perform: complete)
                }
                
                Section(header: Text("Completed")) {
                    ForEach(items, id: \.id) { item in
                        if item.collected {
                            HStack {
                                Text(item.category).strikethrough(item.collected, color: .accentColor)
                                Text(item.name).strikethrough(item.collected, color: .accentColor)
                            }
                        }
                    }
                }
                Text("github.com/JoeyShapiro/Artichoke").foregroundColor(.accentColor)
            }
        }
    }
    
    func complete(at offsets: IndexSet) {
        items[offsets[offsets.startIndex]].collected = true
        makePostCall()
    }
}

struct ContentView_Previews: PreviewProvider {
    static var previews: some View {
        ContentView()
    }
}

private func AddItem(items: inout [Item], name: String, category: String) {
    let item = Item(id: UUID(), category: category, name: name, collected: false)
    
    items.append(item)
}

func makePostCall() {
  let todosEndpoint: String = "http://localhost:6060/items"
  guard let todosURL = URL(string: todosEndpoint) else {
    print("Error: cannot create URL")
    return
  }
  var todosUrlRequest = URLRequest(url: todosURL)
  todosUrlRequest.httpMethod = "POST"
  let newTodo: [String: Any] = ["family": "shapiro", "passphrase": "sha256", "given": "joey"]
  let jsonTodo: Data
  do {
    jsonTodo = try JSONSerialization.data(withJSONObject: newTodo, options: [])
    todosUrlRequest.httpBody = jsonTodo
  } catch {
    print("Error: cannot create JSON from todo")
    return
  }

  let session = URLSession.shared

  let task = session.dataTask(with: todosUrlRequest) {
    (data, response, error) in
    guard error == nil else {
      print("error calling POST on /todos/1")
      print(error!)
      return
    }
    guard let responseData = data else {
      print("Error: did not receive data")
      return
    }

    // parse the result as JSON, since that's what the API provides
    do {
      guard let receivedTodo = try JSONSerialization.jsonObject(with: responseData,
        options: []) as? [String: Any] else {
          print("Could not get JSON from responseData as dictionary")
          return
      }
      print("The todo is: " + receivedTodo.description)

      guard let todoID = receivedTodo["id"] as? Int else {
        print("Could not get todoID as int from JSON")
        return
      }
      print("The ID is: \(todoID)")
    } catch  {
      print("error parsing response from POST on /todos")
      return
    }
  }
  task.resume()
}

func makeGetCall() {
  // Set up the URL request
  let todoEndpoint: String = "http://localhost:6060/items"
  guard let url = URL(string: todoEndpoint) else {
    print("Error: cannot create URL")
    return
  }
  let urlRequest = URLRequest(url: url)
    print("urlRequest.description")

  // set up the session
  let config = URLSessionConfiguration.default
  let session = URLSession(configuration: config)

  // make the request
  let task = session.dataTask(with: urlRequest) {
    (data, response, error) in
    // check for any errors
    guard error == nil else {
      print("error calling GET on /todos/1")
      print(error!)
      return
    }
    // make sure we got data
    guard let responseData = data else {
      print("Error: did not receive data")
      return
    }
    // parse the result as JSON, since that's what the API provides
    do {
      guard let todo = try JSONSerialization.jsonObject(with: responseData, options: [])
        as? [String: Any] else {
          print("error trying to convert data to JSON")
          return
      }
      // now we have the todo
      // let's just print it to prove we can access it
      print("The todo is: " + todo.description)

      // the todo object is a dictionary
      // so we just access the title using the "title" key
      // so check for a title and print it if we have one
      guard let todoTitle = todo["title"] as? String else {
        print("Could not get todo title from JSON")
        return
      }
      print("The title is: " + todoTitle)
    } catch  {
      print("error trying to convert data to JSON")
      return
    }
  }
  task.resume()
}

func apiCall() {
    guard let url = URL(string: "http://localhost:6060/items") else {
        return
    }
    
    var request = URLRequest(url: url)
    
    request.httpMethod = "POST"
    request.setValue("application/json", forHTTPHeaderField: "Content-Type")
    let body: [String: AnyHashable] = ["family_id": "1", "passphrase_hash": "sha256", "given": "joey"]
    request.httpBody = try? JSONSerialization.data(withJSONObject: body, options: .fragmentsAllowed)
    
    let task = URLSession.shared.dataTask(with: request) { data, _, error in
        guard let data = data, error == nil else {
            return
        }
        
        do {
            let response = try JSONSerialization.jsonObject(with: data, options: .allowFragments) as? [String:Any]
            print("SUCCESS", response!)
            let jsonP = response?["data"] as! [String:Any]
            let date = NSDate(timeIntervalSince1970: TimeInterval(jsonP["expires_on"] as! Int))
            print(date)
        }
        catch {
            print(error)
        }
    }
    task.resume()
}
