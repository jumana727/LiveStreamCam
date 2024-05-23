// toast.component.ts
import { Component, Input } from '@angular/core';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-toast',
  templateUrl: './toast.component.html',
  styleUrls: ['./toast.component.css'],
  standalone: true,
  imports: [CommonModule]
})
export class ToastComponent {
  @Input() message: string = '';
  index: number = 0;
  show: boolean = false;

  showToast(message: string) {
    this.message = message;
    this.show = true;
    setTimeout(() => this.show = false, 3000); // Hide after 3 seconds
  }
}
