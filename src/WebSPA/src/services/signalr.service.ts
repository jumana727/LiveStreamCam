import { Injectable } from '@angular/core';
import { HubConnection, HubConnectionBuilder } from '@microsoft/signalr';
import { Observable, Subject } from 'rxjs';
import { GroupMembershipRequest } from '../GroupMembershipRequest';
import { ToastService } from './toast.service';
import { group } from '@angular/animations';

interface AnalyticsResult {
  X: number;
  Y: number;
  Width: number;
  Height: number;
  Score: number;
  status: number;
  dateTime: string;
  Id: string;
}

@Injectable({
  providedIn: 'root'
})

export class SignalrService {

  constructor(private toast : ToastService){}

  hubConnections: { [key: string]: signalR.HubConnection } = {};

  finalAnalyticsResult: AnalyticsResult | null = null;

  startConnection(groupName: GroupMembershipRequest): void {

    console.log("Start signalr connection")

    const hubConnection = new HubConnectionBuilder()
      .withUrl('http://localhost:8080/analyticsResultsHub')
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

      const result: AnalyticsResult = JSON.parse(message);
      console.log("analytics results");
      console.log(result);
      this.finalAnalyticsResult = result;
    });
  }
      // {"X":872.0,"Y":124.0,"Width":434.0,"Height":291.0,"Score":9.1803E-41,"status":0,"dateTime":"2024-07-08T10:23:40.141597Z","Id":"00000000-0000-0000-0000-000000000000"}

  LeaveGroup(groupName: GroupMembershipRequest): void {
    console.log("Leaving Group");
    this.hubConnections[groupName.streamId + groupName.analyticsSettingsId].send('LeaveGroup', groupName);
  }

}

