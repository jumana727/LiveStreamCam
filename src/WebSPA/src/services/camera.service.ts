// camera.service.ts
import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Camera } from '../Camera';


@Injectable({
  providedIn: 'root'
})
export class CameraService {
  private videoStreamApiUrl = 'http://localhost:8080/api/VideoStream';  
  private analyticsApiUrl = 'http://localhost:8080/api/Analytics';
  private hardCodedSettingsId = "7ce26d57-b6fb-463f-adaf-85e6e29dc9cc";

  constructor(private http: HttpClient) {}

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
    return this.http.get<string>(`${this.analyticsApiUrl}/StartAnalytics?videoStreamId=${cameraId}&analyticsSettingsId=${this.hardCodedSettingsId}`)
  }

  stopAnalysis(cameraId: number) : Observable<string> {
    return this.http.get<string>(`${this.analyticsApiUrl}/StopAnalytics?videoStreamId=${cameraId}&analyticsSettingsId=${this.hardCodedSettingsId}`)
  }


}
