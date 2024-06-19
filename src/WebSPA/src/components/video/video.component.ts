// video.component.ts
import { Component, Input, OnChanges, OnInit, Pipe } from '@angular/core';
import { CommonModule } from '@angular/common';
import { DomSanitizer, SafeResourceUrl } from '@angular/platform-browser';

@Component({
  selector: 'app-video',
  templateUrl: './video.component.html',
  styleUrls: ['./video.component.css'],
  standalone: true,
  imports: [CommonModule ]
})

export class VideoComponent implements OnChanges{
  @Input() streamUrl: string = "http://mediamtx:8889/my_camera";

  safeStreamUrl: SafeResourceUrl | undefined;

  constructor(private sanitizer: DomSanitizer) {

  }

  ngOnChanges(): void {
    this.safeStreamUrl = this.sanitizer.bypassSecurityTrustResourceUrl(this.streamUrl);
    console.log("safe stream");
    console.log(this.safeStreamUrl);
  }

}
