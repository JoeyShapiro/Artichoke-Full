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
