import { Component, inject} from '@angular/core';
import { Store } from '@ngrx/store';
import { AsyncPipe } from '@angular/common';
import { selectToasts, ToastType } from '../../services/toast.service/toast.reducer';
import { removeToast } from '../../services/toast.service/toast.action';

@Component({
  selector: 'app-toast',
  imports: [AsyncPipe],
  templateUrl: './toast.component.html',
  styleUrl: './toast.component.css',
})

export class ToastComponent {
  private store = inject(Store);

  toasts$ = this.store.select(selectToasts);

  close(id: string) {
    this.store.dispatch(removeToast({id}))
  }

  iconMap: Record<ToastType, string> = {
    [ToastType.Error]: 'error',
    [ToastType.Warning]: 'warning',
    [ToastType.Success]: 'check_circle',
    [ToastType.Info]: 'info',
  }
}
