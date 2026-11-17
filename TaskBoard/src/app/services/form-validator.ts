import { FormGroup } from '@angular/forms';

export const checkErrors = (form: FormGroup, fieldNames: Record<string, string>) => {
  for (const fieldKey of Object.keys(form.controls)) {
    const control = form.get(fieldKey);

    if (control?.invalid) {
      const readableName = fieldNames[fieldKey] || fieldKey;

      if (control.hasError('required')) {
        return `Field "${readableName}" must be filled.`;
      }
      if (control.hasError('email')){
        return `"${readableName}" must be a valid email address.`;
      }
      if (control.hasError('minlength')){
        return `"${readableName}" must be more than 10 symbols.`;
      }
      if (control.hasError('maxlength')) {
        return `"${readableName}" must be under 50 symbols.`;
      }

      return `Field "${readableName}" filled incorrectly.`;
    }
  }

  return null;
};
