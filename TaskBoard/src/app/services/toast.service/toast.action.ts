import { createAction, props } from "@ngrx/store";
import { ToastType } from "./toast.reducer";

export const showToast = createAction(
  '[toastMessage] Show', 
  props<{id: string; message: string; toastType: ToastType}>()
)

export const removeToast = createAction(
  '[toastMessage] Remove',
  props<{id: string}>()
)
