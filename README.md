# Dealing with eventual consistency

This GitHub repository belongs to a presentation called `Dealing with eventual consistency` by me, [Dennis van der Stelt](https://compilesoftware.nl/). During the presentation I explain what eventual consistency is, why you might run into some of its complexities and how to deal with it. That it's not just about technical solutions, but always requires communication and decision making together with the business stakeholders, and can result in hybrid solutions. Meaning not just technical solutions, but also decisions for change into the business. And how the business returns to being eventual consistent.

I would really appreciate if you took the time to contact me and tell me what you think about the presentation or demo. If there's anything I can help with or explain further, please let me know. I've explained during the presentation distributed systems is where I love to talk about, so don't hesitate to reach out. via my [personal weblog](https://dennis.bloggingabout.net/) or my [freelance business website](https://compilesoftware.nl/).  If you want me to present this or anything else at your company, that is also an option we can discuss.

![](gfx/popcorn-site.png)

## Technical solution

This section explains the solution chosen and why some technologies were included.

### Event Driven Architecture

During the presentation I explain how one can run into eventual consistency. Messaging is one of them. Instead of relying on a database being available, transactions and locks taking place, you can also put a message on the queue and have it processed asynchronously. This makes your application more reliable, balances the load and makes your system better scalable. As a result you do not have immediate access to your data, as it's not stored yet.

I am a developer at Particular Software, makers of NServiceBus. So that was the logical choice to go for. If you want to know more of the benefits, or want a conference call for some architectural guidance, [contact me](https://compilesoftware.nl/).

The browser communicates via SignalR to the website, which then sends a message to a background process, which we'll call an endpoint. The endpoint is hosted in a console application in this example, but can also be hosted as a Windows Service or in Azure or AWS. After the endpoint verifies the movie, it responds back via another message. The website is also hosting an endpoint and converts the incoming message into a SignalR message. The client receives that message and shows the result to the end-user.

### SignalR

The usage of SignalR within this demo is used to basically trick the user into thinking data is processed synchronously. The reason is because users historically have been thinking that this is the way applications work. And sometimes actually have the need for immediate feedback. With SignalR we're mimicking the same behavior, while still benefitting from asynchronous communication.

Together with messaging, as a result, when the website receives high traffic from visitors, our system is able to process messages at a certain pace. Visitors might have to wait a bit longer for a response, but the back-end doesn't get flooded with requests and instead of results, provide the user with an error screen.

Remember though that the user might move on or even closes the browser. As a result, the SignalR connection will be lost. Your messages in the back-end will continue processing, but no one will ever receive the response. This can become a problem if you require more information. But would it be any different if you'd be doing the traditional style of showing the user a new screen and request more data there? Unlikely.

## Upcoming

I plan to work more on this demo

- [Add a database in the back-end](https://github.com/dvdstelt/EventualConsistencyDemo/issues/7)
- Possibly mimic a logged-in user and allow administrative screen
- Publish events in the back-end to show how the system would continue operating
- Actually store comments in the database and verify if comments have been processed and remove those from local storage.

If you have alternative ideas you'd like to see worked out, or maybe even contribute fixing some of my (front-end) code, I'd love to hear from you.
