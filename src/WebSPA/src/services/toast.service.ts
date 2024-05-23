// toast.service.ts
import { Injectable } from '@angular/core';
import { Overlay, OverlayRef } from '@angular/cdk/overlay';
import { ComponentPortal } from '@angular/cdk/portal';
import { ToastComponent } from '../components/toast/toast.component';
ToastComponent

@Injectable({
  providedIn: 'root'
})
export class ToastService {
  private overlayRef: OverlayRef | null = null;
  private toasts: { message: string; overlayRef: OverlayRef }[] = [];

  constructor(private overlay: Overlay) {}

  show(message: string) {
    const overlayRef = this.overlay.create({
      positionStrategy: this.overlay.position()
        .global()
        .top(`${20 + this.toasts.length * 60}px`)
        .right('20px')
    });

    const toastPortal = new ComponentPortal(ToastComponent);
    const toastComponentRef = overlayRef.attach(toastPortal);
    toastComponentRef.instance.message = message;
    toastComponentRef.instance.index = this.toasts.length;

    this.toasts.push({ message, overlayRef });

    setTimeout(() => this.hideToast(overlayRef), 3000); // Hide after 3 seconds
  }

  private hideToast(overlayRef: OverlayRef) {
    const index = this.toasts.findIndex(toast => toast.overlayRef === overlayRef);
    if (index !== -1) {
      this.toasts[index].overlayRef.dispose();
      this.toasts.splice(index, 1);
      this.updateToastPositions();
    }
  }

  private updateToastPositions() {
    this.toasts.forEach((toast, i) => {
      toast.overlayRef.updatePositionStrategy(
        this.overlay.position()
          .global()
          .top(`${20 + i * 60}px`)
          .right('20px')
      );
    });
  }
}
