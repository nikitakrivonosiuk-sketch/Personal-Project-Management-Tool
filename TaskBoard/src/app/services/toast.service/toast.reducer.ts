import { createReducer, on } from "@ngrx/store";
import { removeToast, showToast } from "./toast.action";

// Imports for selectors
import { createFeatureSelector, createSelector } from "@ngrx/store";

export enum ToastType {
    'Error', 'Warning', 'Success', 'Info'
}

export interface Toast {
    id: string;
    message: string;
    toastType: ToastType;
}

export interface ToastState {
    toasts: Toast[],
}

const initialState: ToastState = { toasts: [] }; 

export const toastReducer = createReducer(initialState, 
    on(showToast, (state, action) => ({
        ...state,
        toasts: [...state.toasts, {id: action.id, message: action.message, toastType: action.toastType}]
    })),
    on(removeToast, (state, action) => ({
         ...state,
         toasts: state.toasts.filter(toast => toast.id !== action.id),
    })),
);

// Selectors
export const selectToastState = createFeatureSelector<ToastState>('toast');

export const selectToasts = createSelector(
    selectToastState,
    (state: ToastState) => state.toasts
);