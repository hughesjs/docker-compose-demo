# Step 1

1. Show step 1 design
1. Quickly talk through the WebApp (NotesWebApplicationExtensions.cs)
1. Write docker-compose
1. Demo posting, cache-fetch, cache-miss
1. Demo `netstat -ntlp` and that we're consuming ports on our host
1. Potential for collisions with other apps and/or port exhaustion. Also a pain to scale as each replica needs its own config.
1. Demo lack of persistence

```docker-compose
version: "3.8"
services:
  webapp:
    build:
      context: webapp/DemoWebApp
      dockerfile: Dockerfile
    ports: ["5000:5000"]
  mongo: 
    image: mongo:latest
    ports: ["27017:27017"]
  redis:
    image: redis:latest
    ports: ["6379:6379"]
```

![Step 1 Diagram](notes-assets/step-1.png)

# Step 2

1. Show step 2 design
1. Quickly rehash what volumes are
1. Mention the persistence mountpoints for redis and mongo (`/data` by coincidence)
1. Modify docker-compose
1. Demo that data now survives a restart

```
version: "3.8"
services:
  webapp:
    build:
      context: webapp/DemoWebApp
      dockerfile: Dockerfile
    ports: ["8080:8080"]
  mongo: 
    image: mongo:latest
    ports: ["27017:27017"]
    volumes: 
      - mongodata:/data/db
  redis:
    image: redis:latest
    ports: ["6379:6379"]
    volumes: 
      - redisdata:/data
    
volumes:
    mongodata:
    redisdata:
```docker-compose

```

![Step 2 Diagram](notes-assets/step-2.png)

# Step 3

1. Show step 3 design
1. Discuss how docker manages its own virtual networks
1. Internal networks are only accessible by containers in a specific compose stack
1. External networks are accessible by anything in the docker daemon or on the host
1. Have to manually create external network `docker network create host-network`
1. Internal networks created on `docker-compose up`
1. Modify docker-compose
1. Get IP of webapp using `docker inspect <id> | jq '.[0].NetworkSettings.Networks."host-network".IPAddress'`
1. Demonstrate app doesn't work (ask why, no more ports on host)
1. Modify settings so apps can resolve eachother
1. Demonstrate app now works
1. Demonstrate no host ports used with `netstat -ntlp`

```docker-compose
version: "3.8"
services:
  
  webapp:
    build:
      context: ./webapp/DemoWebApp
      dockerfile: Dockerfile
    networks:
      - host-network
      - internal-network
      
  mongo:
    container_name: mongo
    image: mongo:latest
    volumes: 
      - mongodata:/data/db
    networks:
      - internal-network
        
  redis:
    container_name: redis
    image: redis:latest
    volumes: 
      - redisdata:/data
    networks:
      - internal-network
    
volumes:
    mongodata:
    redisdata:

networks:
  host-network:
    external: true
  internal-network:
    external: false
```

![Step 3 Diagram](notes-assets/step-3.png)
