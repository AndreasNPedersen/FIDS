import React, { useEffect, useState } from 'react';
import Table from 'react-bootstrap/Table';
import Container from 'react-bootstrap/Container';


let socket = new WebSocket("ws://localhost:8010/");
function DataTableDeparture() {
  const [data, setData] = useState([])
  useEffect(()=> {
    socket.onopen = function () { console.log("success");socket.send("Departure") };
    socket.onmessage = function (msg) {
      var dataIncoming = JSON.parse(msg.data);
      setData(dataIncoming)
      console.log(dataIncoming)
    };
    socket.onclose = function () { console.log("closed"); socket.send("DepartureClose") };

  },[data])



  return (
    <Container>
      <Table striped bordered hover size="xxl" variant="dark">
        <thead>
          <tr>
            <th>Afgangstid</th>
            <th>Til Lufthavn</th>
            <th>Selskab</th>
            <th>Fly rejse Id</th>
            <th>Status</th>
            <th>Gate</th>
          </tr>
        </thead>
        <tbody>
          {data.map(element => {
        return <tr>
          <td>{element.DepartureTime}</td>
          <td>{element.ToAirport}</td>
          <td>{element.AirplaneOwner}</td>
          <td>{element.FlightJourneyId}</td>
          <td>{element.Status}</td>
          <td>{element.Gate}</td>
        </tr>
      })}
        </tbody>
      </Table>
    </Container>
  );
}

export default DataTableDeparture;
