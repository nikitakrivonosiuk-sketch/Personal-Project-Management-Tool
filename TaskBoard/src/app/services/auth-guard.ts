import { inject } from '@angular/core';
import { CanActivateChildFn, Router } from '@angular/router';
import { TokenStorageService } from './token-storage-service';

export const authGuard: CanActivateChildFn = () => {
  const router = inject(Router);
  const tokenStorage = inject(TokenStorageService);

  if (tokenStorage.getToken()){
    return true;
  }

  return router.createUrlTree(["/login"]);
};
