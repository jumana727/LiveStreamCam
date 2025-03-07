// camera.component.ts
import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { HttpClientModule } from '@angular/common/http';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatListModule } from '@angular/material/list';
import { MatIconModule } from '@angular/material/icon';
import { MatCardModule } from '@angular/material/card';

import { Camera } from '../../Camera';
import { CameraService } from '../../services/camera.service';
import { VideoComponent } from '../video/video.component';
import { ToastService } from '../../services/toast.service';

@Component({
  selector: 'app-camera',
  templateUrl: './camera.component.html',
  styleUrls: ['./camera.component.css'],
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    HttpClientModule,
    MatInputModule,
    MatButtonModule,
    MatListModule,
    MatIconModule,
    MatCardModule,
    VideoComponent
  ],
  providers: [CameraService]
})
export class CameraComponent implements OnInit {
  cameras: Camera[] = [];
  cameraForm: FormGroup;
  isEditing: boolean = false;

  currentCameraId: number = 0;
  

  selectedStream: string = "";
  analyticsType: string = "";


  constructor(private cameraService: CameraService, private fb: FormBuilder, private toastService: ToastService) {
    this.cameraForm = this.fb.group({
      uri: ['']
    });
  }

  ngOnInit(): void {
    this.loadCameras();
  }

  loadCameras(): void {
    this.cameraService.getCameras().subscribe((data) => {
      this.cameras = data;
    });
  }

  saveCamera(): void {
    if (this.isEditing && this.currentCameraId !== null) {
      this.cameraService.updateCamera(this.currentCameraId, this.cameraForm.value.uri).subscribe(() => {
        this.loadCameras();
        this.resetForm();
        this.toastService.show("Camera Edited Successfully!");
      });
    } else {
      this.cameraService.createCamera(this.cameraForm.value.uri).subscribe(() => {
        this.loadCameras();
        this.resetForm();
        this.toastService.show("New Camera Created Successfully!");
      });
    }
  }

  editCamera(camera: Camera): void {
    this.isEditing = true;
    this.currentCameraId = camera.id;
    this.cameraForm.patchValue(camera);
  }

  deleteCamera(id: number): void {
    this.cameraService.deleteCamera(id).subscribe(() => {
      this.loadCameras();
      this.toastService.show("Camera Deleted Successfully!");
    });
  }

  resetForm(): void {
    this.isEditing = false;
    this.currentCameraId = 0;
    this.cameraForm.reset();
  }

  playStream(camera: Camera): void {
    console.log("play stream");
    console.log(camera.uri);
   
    const sourceName = this.getWebRTCUrl(camera.uri);

    this.cameraService.playWebRTCStream(sourceName, camera);

    this.selectedStream = "http://localhost:8889/" + sourceName
    this.currentCameraId = camera.id;

    console.log("webrtc stream ", this.selectedStream);
  }

  stopStream(camera: Camera) : void {
    this.cameraService.stopWebRTCStream(this.getWebRTCUrl(camera.uri))
  } 

  getWebRTCUrl(url: string) : string{
    let temp = url.split("//")[1]
    let path = temp.split(':')[1].split('/')[1];

    return path + "-webrtc";

  }

  setAnalyticsType(analyticsType: string) : void {
    this.analyticsType = analyticsType;
  }

  startAnalysis() {
    this.cameraService.startAnalysis(this.currentCameraId).subscribe(() => {
      console.log("starting analyssis!!!");
    });
  }

  stopAnalysis() {
    this.cameraService.stopAnalysis(this.currentCameraId).subscribe(() => {
      console.log("stopping analyssis!!!");
    });
  }

}
