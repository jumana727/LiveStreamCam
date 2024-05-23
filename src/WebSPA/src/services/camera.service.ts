// camera.service.ts
import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Camera } from '../Camera';


@Injectable({
  providedIn: 'root'
})
export class CameraService {
  private apiUrl = 'http://localhost:3000/cameras';  // Replace with your API URL

  constructor(private http: HttpClient) {}

  getCameras(): Observable<Camera[]> {
    return this.http.get<Camera[]>(this.apiUrl);
  }

  getCamera(id: number): Observable<Camera> {
    return this.http.get<Camera>(`${this.apiUrl}/${id}`);
  }

  createCamera(camera: Camera): Observable<Camera> {
    return this.http.post<Camera>(this.apiUrl, camera);
  }

  updateCamera(camera: Camera): Observable<Camera> {
    return this.http.put<Camera>(`${this.apiUrl}/${camera.id}`, camera);
  }

  deleteCamera(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }
}
