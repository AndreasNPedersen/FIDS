# FIDS

```mermaid
flowchart
    subgraph dg[Diplom Gruppe]
        dfids[./compose-fids.yml]
        
        subgraph fb[FIDS Backend]
            arrapi[Arrivals Controller]
            depapi[Departures Controller]
            ah[Arrivals Hub]
            dh[Departures Hub]
            aw[Arrivals Worker]
            dw[Departures Worker]
            
        end
        
        ac[FIDS Arrivals Client]
        dc[FIDS Departures Client]

        ah --ReceiveArrivalStatus--> ac
        dh --ReceiveDepartureStatus--> dc

        aw --ReceiveArrivalStatus--> ac
        dw --ReceiveDepartureStatus--> dc
        
        ac --/Arrivals--> arrapi
        dc --/Departures--> depapi
        
        fly[Fly]
        fr[Fly Rejser]
        
        dfids -.starts.-> fb
        dfids -.starts.-> ac
        dfids -.starts.-> dc
    end
    
    subgraph Vejle 
        
    end
    
    subgraph Gruppe 1

    end
    
    subgraph Gruppe 2

    end
```

```powershell
    # Build
    docker compose -f .\compose-fids.yml build --no-cache
    
    # Start
    docker compose -f .\compose-fids.yml up -d
    
    # Stop and remove
    docker compose -f .\compose-fids.yml down
```

- After startup go to `http://localhost:5601` (Kibana dashboard)
- In Index pattern change `logstash-*` to `fids-*`
- Choose @timestamp in Time Filter field name
- Click Create
- Go to the Discover tab