import { Injectable, inject } from '@angular/core';
import { Store } from '@ngrx/store';
import { showToast } from './toast.action';
import { ToastType } from './toast.reducer';

@Injectable({ providedIn: 'root' })
export class ToastService {
  private store = inject(Store);

  success(message: string) {
    this.store.dispatch(showToast({ id: crypto.randomUUID(), message, toastType: ToastType.Success }));
  }

  error(message: string) {
    this.store.dispatch(showToast({ id: crypto.randomUUID(), message, toastType: ToastType.Error}));
  }

  info(message: string) {
    this.store.dispatch(showToast({id: crypto.randomUUID(), message, toastType: ToastType.Info}))
  }

  warning(message: string) {
    this.store.dispatch(showToast({id: crypto.randomUUID(), message, toastType: ToastType.Warning}))
  }
}