// video.component.ts
import { AfterViewInit, Component, ElementRef, Input, OnChanges, OnInit, ViewChild } from '@angular/core';
import { CommonModule } from '@angular/common';
import { DomSanitizer, SafeResourceUrl } from '@angular/platform-browser';
import { SignalrService } from '../../services/signalr.service';

@Component({
  selector: 'app-video',
  templateUrl: './video.component.html',
  styleUrls: ['./video.component.css'],
  standalone: true,
  imports: [CommonModule]
})
export class VideoComponent implements OnChanges, AfterViewInit, OnInit {
  @Input() streamUrl: string = "http://localhost:8889/my_camera";
  @ViewChild('overlayCanvas', { static: false }) overlayCanvas!: ElementRef<HTMLCanvasElement>;
  
  safeStreamUrl: SafeResourceUrl | undefined;

  // Assuming these are the original dimensions of the OpenCV image
  private opencvWidth: number = 1280;
  private opencvHeight: number = 720;

  private canvasIntervalMilliSecond: number = 10;

  constructor(private sanitizer: DomSanitizer, private signalrService: SignalrService) {}

  ngOnInit(): void {
    // Initialize signalR connection or any other setup logic
  }

  ngAfterViewInit(): void {
    const canvas = this.overlayCanvas.nativeElement;
    const context = canvas.getContext('2d');

    if (!canvas || !context) {
      console.error('Canvas or context not found!');
      return;
    }

    // Adjust canvas size to match the video container
    const videoContainer = canvas.parentElement;
    canvas.width = videoContainer!.clientWidth;
    canvas.height = videoContainer!.clientHeight;

    console.log('Canvas dimensions:', canvas.width, canvas.height);

    // Calculate scaling factors
    const scaleX = canvas.width / this.opencvWidth;
    const scaleY = canvas.height / this.opencvHeight;

    // Function to draw a rectangle
    const drawRectangleOnCanvas = (x: number, y: number, width: number, height: number) => {
      context.clearRect(0, 0, canvas.width, canvas.height); // Clear previous drawings

      // Convert OpenCV coordinates to Canvas coordinates
      const canvasX = x * scaleX;
      const canvasY = y * scaleY;
      const canvasWidth = width * scaleX;
      const canvasHeight = height * scaleY;

      console.log('Drawing box at:', canvasX, canvasY, 'with dimensions:', canvasWidth, canvasHeight);

      context.strokeStyle = 'red';
      context.lineWidth = 1;
      context.strokeRect(canvasX, canvasY, canvasWidth, canvasHeight);
    };

    setInterval(() => {
      const result = this.signalrService.finalAnalyticsResult;

      if (result) {
        const x = result.X ?? 0;
        const y = result.Y ?? 0;
        const width = result.Width ?? 0;
        const height = result.Height ?? 0;
        drawRectangleOnCanvas(x, y, height, width);
      }
    }, this.canvasIntervalMilliSecond);
  }

  ngOnChanges(): void {
    this.safeStreamUrl = this.sanitizer.bypassSecurityTrustResourceUrl(this.streamUrl);
    console.log("safe stream");
    console.log(this.safeStreamUrl);
  }
}
