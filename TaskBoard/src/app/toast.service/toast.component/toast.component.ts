import { Component, inject} from '@angular/core';
import { Store } from '@ngrx/store';
import { selectToasts } from '../toast.reducer';
import { removeToast } from '../toast.action';
import { AsyncPipe } from '@angular/common';

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

  iconMap: Record<number, string> = {
    0: 'error',
    1: 'warning',
    2: 'check_circle',
    3: 'info',
  }
}
