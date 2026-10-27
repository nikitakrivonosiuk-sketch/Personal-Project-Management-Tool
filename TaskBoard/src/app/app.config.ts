import { ApplicationConfig, provideBrowserGlobalErrorListeners } from '@angular/core';
import { provideRouter } from '@angular/router';

import { routes } from './app.routes';
import { provideHttpClient } from '@angular/common/http';
import { provideStore } from '@ngrx/store';
import { provideEffects } from '@ngrx/effects';
import { toastReducer } from './services/toast.service/toast.reducer';
import { ToastEffects } from './services/toast.service/toast.effects';
import { BoardEffects } from './state/board.effects';
import { boardReducer } from './state/board.reducer';

export const appConfig: ApplicationConfig = {
  providers: [
    provideBrowserGlobalErrorListeners(),
    provideRouter(routes),
    provideHttpClient(),
    provideStore({
      toast: toastReducer,
      boardFeature: boardReducer
    }),
    provideEffects([ToastEffects]),
    provideEffects([BoardEffects])
  ],
};
