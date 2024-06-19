import { Injectable } from '@angular/core';
import { HubConnection, HubConnectionBuilder } from '@microsoft/signalr';
import { Observable, Subject } from 'rxjs';
import { GroupMembershipRequest } from '../GroupMembershipRequest';
import { ToastService } from './toast.service';
import { group } from '@angular/animations';

@Injectable({
  providedIn: 'root'
})

export class SignalrService {

  constructor(private toast : ToastService){}

  hubConnections: { [key: string]: signalR.HubConnection } = {};

  startConnection(groupName: GroupMembershipRequest): void {

    console.log("Start signalr connection")

    const hubConnection = new HubConnectionBuilder()
      .withUrl('http://publicapi:8080/analyticsResultsHub')
      .build();
    this.hubConnections[groupName.streamId + groupName.analyticsSettingsId] = hubConnection;

    this.hubConnections[groupName.streamId + groupName.analyticsSettingsId]
      .start()
      .then(() => {
        console.log('Connection started');
        this.addClientToGroup(groupName);
      })
      .then(() =>{
        this.recieveMembershipResponse(groupName);
      })
      .then(() => {
        console.log("seomthing");
        this.recieveAnalyticsNotification(groupName);
      })
      .catch(err => console.log('Error while starting connection: ' + err));
  }

  recieveMembershipResponse(groupName: GroupMembershipRequest): void {
    this.hubConnections[groupName.streamId + groupName.analyticsSettingsId].on('MembershipResponse', (response)=>{
      console.log(response);
      this.toast.show(response);
    })
  }

  addClientToGroup(groupName: GroupMembershipRequest): void {
    console.log("add client to group");
    this.hubConnections[groupName.streamId + groupName.analyticsSettingsId].send('JoinGroup', groupName);

  }

  recieveAnalyticsNotification(groupName: GroupMembershipRequest): void {
    console.log("Adding signalr message Listener")
    console.log(groupName.streamId + groupName.analyticsSettingsId);
    this.hubConnections[groupName.streamId + groupName.analyticsSettingsId].on('result', (message) => {
      console.log(message);
      this.toast.show(message);
    });
  }

  LeaveGroup(groupName: GroupMembershipRequest): void {
    console.log("Leaving Group");
    this.hubConnections[groupName.streamId + groupName.analyticsSettingsId].send('LeaveGroup', groupName);
  }

}
