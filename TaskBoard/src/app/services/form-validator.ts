import { FormGroup } from '@angular/forms';

export const checkErrors = (form: FormGroup, fieldNames: Record<string, string>) => {
  for (const fieldKey of Object.keys(form.controls)) {
    const control = form.get(fieldKey);

    if (control?.invalid) {
      const readableName = fieldNames[fieldKey] || fieldKey;

      if (control.hasError('required')) {
        return `Field "${readableName}" must be filled.`;
      }
      if (control.hasError('maxlength')) {
        return `"${readableName}" must be under 50 symbols.`;
      }

      return `Field "${readableName}" filled incorrectly.`;
    }
  }

  return null;
};
