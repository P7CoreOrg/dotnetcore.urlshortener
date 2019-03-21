# dotnetcore.urlshortener

This is a typical url shortener service that has abstracted out storage, configuration, and events that you need to do analytics.

A Url Shortener Service is really more about those cool dashboards and reports that marketing folks need when they run campaigns.

Those reports and dashboards are not this.

This is a chatty service that only implements the following activities;
  * Turning long urls into short urls
  * Storing the resultant record and giving you back a lookup id
  * Fetching the store record
  * Removing the stored record
  * Reacting to expired records by requiring that you provide an url when that happens
  * A pub-sub that tells you when important things happen for a rich reporting service to tap into
  

Currently I have an InMemory Working version, but it is expected that a production variant will have stuff like;
  * a fast backing database like redis
  * a rich usage analytics service
    This one is multiple factors bigger and more complicated than this thing
    
  
# Replaceable Stores
