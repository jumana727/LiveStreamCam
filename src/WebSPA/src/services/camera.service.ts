// camera.service.ts
import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Camera } from '../Camera';

import { GroupMembershipRequest } from "../GroupMembershipRequest";
import { SignalrService } from './signalr.service';


@Injectable({
  providedIn: 'root'
})
export class CameraService {
  private videoStreamApiUrl = 'http://localhost:8080/api/VideoStream';  
  private analyticsApiUrl = 'http://localhost:8080/api/Analytics';
  private hardCodedSettingsId = "7ce26d57-b6fb-463f-adaf-85e6e29dc9cc";
  private mediamtxControlApiEndpint = "http://localhost:9997/v3/config/paths"

  constructor(private http: HttpClient, private signalrService: SignalrService) {}

  getCameras(): Observable<Camera[]> {
    return this.http.get<Camera[]>(this.videoStreamApiUrl);
  }

  getCamera(id: number): Observable<Camera> {
    return this.http.get<Camera>(`${this.videoStreamApiUrl}/${id}`);
  }

  createCamera(uri: string): Observable<string> {

    const headers = new HttpHeaders({
      'Content-Type': 'application/json',
      'Accept': 'text/plain'
    });

    return this.http.post<string>(this.videoStreamApiUrl, JSON.stringify(uri), { headers });
  }
  
  updateCamera(cameraId: number, cameraUri: string): Observable<string> {
    return this.http.put<string>(`${this.videoStreamApiUrl}?VideoStreamId=${cameraId}&VideoStreamUri=${cameraUri}`, cameraId);
  }

  deleteCamera(id: number): Observable<void> {
    return this.http.delete<void>(`${this.videoStreamApiUrl}/${id}`);
  }


  startAnalysis(cameraId: number) : Observable<string>{
    var request : GroupMembershipRequest = {
      streamId : cameraId.toString(),
      analyticsSettingsId : this.hardCodedSettingsId
    };
  
    this.signalrService.startConnection(request);
    let result = this.http.get<string>(`${this.analyticsApiUrl}/StartAnalytics?videoStreamId=${cameraId}&analyticsSettingsId=${this.hardCodedSettingsId}`)
    return result;

  }

  stopAnalysis(cameraId: number) : Observable<string> {
    var request : GroupMembershipRequest = {
      streamId : cameraId.toString(),
      analyticsSettingsId : this.hardCodedSettingsId
    };
    this.signalrService.LeaveGroup(request);
    return this.http.get<string>(`${this.analyticsApiUrl}/StopAnalytics?videoStreamId=${cameraId}&analyticsSettingsId=${this.hardCodedSettingsId}`)
  }

  playWebRTCStream(streamName: string, rtspUri: string) : void{
    this.http.post(`${this.mediamtxControlApiEndpint}/add/${streamName}`, {
      "name": streamName ,
      "source": rtspUri
    })
    .subscribe(data => {
      console.log(data)
    })
  }

  stopWebRTCStream(streamName: string) : void {
    this.http.delete(`${this.mediamtxControlApiEndpint}/delete/${streamName}`).subscribe(() => {
      console.log("stopping web-rtc stream")
    })
  }
}


