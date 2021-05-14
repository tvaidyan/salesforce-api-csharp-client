# salesforce-api-csharp-client

This C# console application is a demo app that demonstrates how to connect to your Salesforce org, using OAuth 2.0 JWT Bearer Flow for Server-to-Server Integration.  You can learn more about this from a series of blog posts that I wrote on the subject, here:
https://tvaidyan.medium.com/create-your-own-rest-api-endpoints-in-salesforce-using-apex-rest-3292cfa1e1af

# How to run this demo app?
To run this demo app, you have a handful of prerequisites:
- Have a Salesforce Org to connect to.
- Have one or more API endpoints setup to call.
- Have a TLS certificate (perhaps generated through OpenSSL).
- Have a "Connected App" setup and configured within your Salesforce org.

Once you specify the relevant information in the settings of this app, you can run it and it should connect to your Salesforce and push/pull back data that you have specified.  Once again, this repo is a companion to the blog entries that I made on the subject.  Click the link above, to get more info.
