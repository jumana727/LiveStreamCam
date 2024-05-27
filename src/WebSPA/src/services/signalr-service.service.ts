import { Injectable } from '@angular/core';
import { HubConnection, HubConnectionBuilder } from '@microsoft/signalr';
import { Observable, Subject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})

export class SignalrServiceService {


  hubConnections: { [key: string]: signalR.HubConnection } = {};

  startConnection(groupName: string): void {

    const hubConnection = new HubConnectionBuilder()
      .withUrl('http://localhost:5008/streamhub')
      .build();
    this.hubConnections[groupName] = hubConnection;

    this.hubConnections[groupName]
      .start()
      .then(() => {
        console.log('Connection started');
        this.addClientToGroup(groupName);
      })
      .catch(err => console.log('Error while starting connection: ' + err));
  }

  addClientToGroup(groupName: string): void {
    console.log("add client to group");
    this.hubConnections[groupName].send('AddClient', groupName);
  }

  receiveLiveStreamListener(groupName: string): void {
    this.hubConnections[groupName].on('receiveData', (message) => {
      console.log(message);
    });
  }

}
