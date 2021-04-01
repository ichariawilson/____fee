### **Prerequisite**
* Installed Docker Desktop for Windows as directed in https://hub.docker.com/editions/community/docker-ce-desktop-windows/.
* Installed Git from https://git-scm.com/download/win. 
* Installed Powershell from https://github.com/PowerShell/PowerShell/releases/download/v7.1.2/PowerShell-7.1.2-win-x64.msi.

### Building and deploying
#### a. Create a folder for your repositories
```
PS C:\Users\<username>\Desktop
```

#### b. Clone Fee's GitHub repo
```
git clone https://github.com/lishadeve/_fee.git
```

#### c. Build the application(You must have docker running)
```
PS C:\Users\<username>\Desktop\_fee>

PS C:\Users\<username>\Desktop\_fee> docker-compose build
```
This build process should take between 10 and 30 minutes to complete, depending on your system's speed.

#### d. Deploy to the local Docker host
```
PS C:\Users\<username>\Desktop\_fee> docker-compose up sqldata
PS C:\Users\<username>\Desktop\_fee> docker-compose up seq
PS C:\Users\<username>\Desktop\_fee> docker-compose up nosqldata
PS C:\Users\<username>\Desktop\_fee> docker-compose up basketdata
PS C:\Users\<username>\Desktop\_fee> docker-compose up rabbitmq
PS C:\Users\<username>\Desktop\_fee> docker-compose up mobileapplyingapigw
```
For each of these commands, Ctrl+C to stop each containers after it's successfully running.
```
PS C:\Users\<username>\Desktop\_fee> docker-compose start sqldata
PS C:\Users\<username>\Desktop\_fee> docker-compose start seq
PS C:\Users\<username>\Desktop\_fee> docker-compose start nosqldata
PS C:\Users\<username>\Desktop\_fee> docker-compose start basketdata
PS C:\Users\<username>\Desktop\_fee> docker-compose start rabbitmq
PS C:\Users\<username>\Desktop\_fee> docker-compose start mobileapplyingapigw
```

```
PS C:\Users\<username>\Desktop\_fee> docker-compose up corporatesponsoridentity-api
PS C:\Users\<username>\Desktop\_fee> docker-compose up student-identity-api
PS C:\Users\<username>\Desktop\_fee> docker-compose up sponsor-api
PS C:\Users\<username>\Desktop\_fee> docker-compose up scholarship-api
PS C:\Users\<username>\Desktop\_fee> docker-compose up applying-basket-api
PS C:\Users\<username>\Desktop\_fee> docker-compose up applying-api
PS C:\Users\<username>\Desktop\_fee> docker-compose up applying-backgroundtasks
PS C:\Users\<username>\Desktop\_fee> docker-compose up payment-api
PS C:\Users\<username>\Desktop\_fee> docker-compose up webhooks-api
PS C:\Users\<username>\Desktop\_fee> docker-compose up webhooks-client
PS C:\Users\<username>\Desktop\_fee> docker-compose up webapplyingapigw
PS C:\Users\<username>\Desktop\_fee> docker-compose up mobileapplyingagg
PS C:\Users\<username>\Desktop\_fee> docker-compose up webapplyingagg
PS C:\Users\<username>\Desktop\_fee> docker-compose up applying-signalrhub
PS C:\Users\<username>\Desktop\_fee> docker-compose up webspa
PS C:\Users\<username>\Desktop\_fee> docker-compose up webmvc
PS C:\Users\<username>\Desktop\_fee> docker-compose up webstatus
```

For each of these commands, Ctrl+C to stop each containers after it's successfully running.

```
PS C:\Users\<username>\Desktop\_fee> docker-compose start corporatesponsoridentity-api
PS C:\Users\<username>\Desktop\_fee> docker-compose start student-identity-api
PS C:\Users\<username>\Desktop\_fee> docker-compose start sponsor-api
PS C:\Users\<username>\Desktop\_fee> docker-compose start scholarship-api
PS C:\Users\<username>\Desktop\_fee> docker-compose start applying-basket-api
PS C:\Users\<username>\Desktop\_fee> docker-compose start applying-api
PS C:\Users\<username>\Desktop\_fee> docker-compose start applying-backgroundtasks
PS C:\Users\<username>\Desktop\_fee> docker-compose start payment-api
PS C:\Users\<username>\Desktop\_fee> docker-compose start webhooks-api
PS C:\Users\<username>\Desktop\_fee> docker-compose start webhooks-client
PS C:\Users\<username>\Desktop\_fee> docker-compose start webapplyingapigw
PS C:\Users\<username>\Desktop\_fee> docker-compose start mobileapplyingagg
PS C:\Users\<username>\Desktop\_fee> docker-compose start webapplyingagg
PS C:\Users\<username>\Desktop\_fee> docker-compose start applying-signalrhub
PS C:\Users\<username>\Desktop\_fee> docker-compose start webspa
PS C:\Users\<username>\Desktop\_fee> docker-compose start webmvc
PS C:\Users\<username>\Desktop\_fee> docker-compose start webstatus
```

Run the following command to list all containers and expose their ports(4 digit number starting with 5).

```
PS C:\Users\<username>\Desktop\_fee> docker container ls -a
```

#### At this point you should be able to navigate to any service. For instance, the WebStatus microservice on http://localhost:5203/.
