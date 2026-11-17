import { Component, inject } from '@angular/core';
import { FormControl, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { ToastService } from '../../services/toast.service/toast.service';
import { AuthService } from '../../services/auth-Service';
import { checkErrors } from '../../services/form-validator';

@Component({
  selector: 'app-register-component',
  imports: [RouterLink, ReactiveFormsModule],
  templateUrl: './register-component.html',
  styleUrl: './register-component.css',
})
export class RegisterComponent {
  registerForm = new FormGroup({
    email: new FormControl('', { nonNullable: true ,validators: [Validators.required, Validators.email]}),
    password: new FormControl('', {nonNullable: true, validators:[Validators.required, Validators.minLength(10)]}),
  });

  private toaster = inject(ToastService);
  private router = inject(Router);
  private authService = inject(AuthService);

  onSubmit(){
    const dictionary = {
      email: "Email",
      password: "Password",
    };

    const isValid = checkErrors(this.registerForm, dictionary);

    if (isValid !== null){
      this.registerForm.markAllAsTouched();
      this.toaster.warning(isValid);
      return;
    }

    const {email, password} = this.registerForm.getRawValue();

    this.authService.registerUser(email, password).subscribe({
      next: () => {
        this.router.navigate(["/login"]);
      },
      error: (err) => {
        if (err.status === 409){
          this.toaster.error("This email is already signed up.");
          return;
        }

        this.toaster.error('Account was not created.');
      },
    })
  }
}
