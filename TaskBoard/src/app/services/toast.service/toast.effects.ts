import { inject, Injectable } from "@angular/core";
import { Actions, createEffect, ofType } from "@ngrx/effects";
import { removeToast, showToast } from "./toast.action";
import { delay, map } from "rxjs";

@Injectable()
export class ToastEffects {
    private actions$ = inject(Actions);

    autoCloseToast$ = createEffect(() => 
        this.actions$.pipe(
            ofType(showToast),
            delay(3000),
            map(action => removeToast({id: action.id}))
        )
    );
}