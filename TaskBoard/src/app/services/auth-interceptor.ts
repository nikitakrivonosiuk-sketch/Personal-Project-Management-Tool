import { inject } from '@angular/core';
import { TokenStorageService } from './token-storage-service';
import { HttpErrorResponse, HttpInterceptorFn} from '@angular/common/http';
import { catchError, throwError } from 'rxjs';
import { Router } from '@angular/router';
import { Store } from '@ngrx/store';
import { BoardActions } from '../state/board.actions';
import { BoardService } from './board-service';
import { ToastService } from './toast.service/toast.service';

export const authInterceptor: HttpInterceptorFn = (req, next) => {
    const tokenStorage = inject(TokenStorageService);  
    const toaster = inject(ToastService);  
    const store = inject(Store);
    const token = tokenStorage.getToken();
    const router = inject(Router);
    const boardService = inject(BoardService);

    if (!token){
        return next(req);
    }

    const authClone = req.clone({
        setHeaders: {
            Authorization: `Bearer ${token}`,
        },
    });

    return next(authClone).pipe(
        catchError((error : unknown) => {
            if (error instanceof HttpErrorResponse && error.status === 401){
                toaster.info("Your session is expired. Login again.");
                tokenStorage.clearToken();
                store.dispatch(BoardActions.logout());
                boardService.activeBoardId.set(null);
                void router.navigate(["/login"]);
            }

            return throwError(() => error);
        })
    );
};
