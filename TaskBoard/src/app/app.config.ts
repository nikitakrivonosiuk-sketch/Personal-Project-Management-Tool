  import { ApplicationConfig, provideBrowserGlobalErrorListeners } from '@angular/core';
  import { provideRouter } from '@angular/router';

  import { routes } from './app.routes';
  import { provideHttpClient, withInterceptors } from '@angular/common/http';
  import { provideStore } from '@ngrx/store';
  import { provideEffects } from '@ngrx/effects';
  import { toastReducer } from './services/toast.service/toast.reducer';
  import { ToastEffects } from './services/toast.service/toast.effects';
  import { BoardEffects } from './state/board.effects';
  import { boardMembersReducer, boardReducer } from './state/board.reducer';
  import { authInterceptor } from './services/auth-interceptor';

  export const appConfig: ApplicationConfig = {
    providers: [
      provideBrowserGlobalErrorListeners(),
      provideRouter(routes),
      provideHttpClient(withInterceptors([authInterceptor])),
      provideStore({
        toast: toastReducer,
        boardFeature: boardReducer,
        boardMembersFeature: boardMembersReducer
      }),
      provideEffects([ToastEffects]),
      provideEffects([BoardEffects])
    ],
  };
