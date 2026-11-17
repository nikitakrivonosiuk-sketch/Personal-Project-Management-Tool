import { Component, inject } from '@angular/core';
import { FormControl, FormGroup,  ReactiveFormsModule, Validators } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { AuthService } from '../../services/auth-Service';
import { checkErrors } from '../../services/form-validator';
import { ToastService } from '../../services/toast.service/toast.service';
import { HttpErrorResponse } from '@angular/common/http';

@Component({
  selector: 'app-login-component',
  imports: [RouterLink, ReactiveFormsModule],
  templateUrl: './login-component.html',
  styleUrl: './login-component.css',
})
export class LoginComponent {
  loginForm = new FormGroup({
    email: new FormControl('',{ nonNullable: true ,validators: [Validators.required, Validators.email]}),
    password: new FormControl('', {nonNullable: true, validators:[Validators.required]}),
  });

  private authService = inject(AuthService);
  private toaster = inject(ToastService);
  private router = inject(Router);

  onSubmit(){
    const dictionary = {
      email: "Email",
      password: "Password",
    };

    const isValid = checkErrors(this.loginForm, dictionary);

    if (isValid !== null) {
      this.loginForm.markAllAsTouched();
      this.toaster.warning(isValid);
      return;
    }

    const { email, password } = this.loginForm.getRawValue();

    this.authService.loginUser(email, password).subscribe({
      next: () => {
        this.router.navigate(["boards"]);
      },
      error: (err : HttpErrorResponse) => {
        if (err.status === 401) this.toaster.error("Email or password is incorrect.");
        else if (err.status === 0) this.toaster.error("Failed to connect to server.");
        else this.toaster.error("Failed to login. Try later.")
      }
    })
    
  }
}
