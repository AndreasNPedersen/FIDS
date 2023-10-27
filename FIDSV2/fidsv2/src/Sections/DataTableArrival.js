import React, { useEffect, useState } from 'react';
import Table from 'react-bootstrap/Table';
import Container from 'react-bootstrap/Container';

let socket = new WebSocket("ws://localhost:8010/");
function DataTableArrival() {
  const [data, setData] = useState([])
  useEffect(()=> {
    socket.onopen = function () { console.log("success"); socket.send("Arrival") };
    socket.onmessage = function (msg) {
      var dataIncoming = JSON.parse(msg.data);
      setData(dataIncoming)
      console.log(dataIncoming)
    };
    socket.onclose = function () { console.log("closed"); socket.send("ArrivalClose") };

  },[data])



  return (
    <Container>
      <Table striped bordered hover size="xxl" variant="dark">
        <thead>
          <tr>
            <th>Ankomst</th>
            <th>Fra Lufthavn</th>
            <th>Selskab</th>
            <th>Fly rejse Id</th>
            <th>Status</th>
            <th>Baggage Gate</th>
          </tr>
        </thead>
        <tbody>
          {data.map(element => {
        return <tr>
          <td>{element.ArrivalDate}</td>
          <td>{element.FromLocation}</td>
          <td>{element.AirplaneOwner}</td>
          <td>{element.FlightJourneyId}</td>
          <td>{element.Status}</td>
          <td>{element.BagageClaimGate}</td>
        </tr>
      })}
        </tbody>
      </Table>
    </Container>
  );
}

export default DataTableArrival;
