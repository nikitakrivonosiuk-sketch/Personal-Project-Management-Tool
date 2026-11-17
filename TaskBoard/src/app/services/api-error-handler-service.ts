import { inject, Injectable } from "@angular/core";
import { ToastService } from "./toast.service/toast.service";
import { HttpErrorResponse } from "@angular/common/http";

@Injectable({providedIn: 'root'})
export class ApiErrorHandlerService {
    private toaster = inject(ToastService);

    handle(error: HttpErrorResponse, fallback: string): string{
        const message = this.getErrorMessage(error, fallback);

        if (error.status !== 401)
            this.toaster.error(message);
        
        return message;
    }

    private getErrorMessage(error: HttpErrorResponse, fallback: string){
        if (error.status === 401) return "Session expired. Login again to continue.";
        if (error.status === 403) return 'You do not have permission to do this.';
        if (error.status === 404) return 'The requested resource was not found.';
        if (error.status === 409) return error.error?.detail ?? 'This action conflicts with current data.';

        return fallback;
    }
}