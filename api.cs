using System.Net.Http;  
  var values = new Dictionary<string, string>
  {
      { "thing1", "hello" },
      { "thing2", "world" }
  };

  var content = new FormUrlEncodedContent(values);

  var response = await client.PostAsync("http://www.example.com/recepticle.aspx", content);

  var responseString = await response.Content.ReadAsStringAsync();