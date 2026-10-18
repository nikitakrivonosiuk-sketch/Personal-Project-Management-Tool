import { Component, EventEmitter, inject, Input, OnChanges, Output, SimpleChanges } from '@angular/core';
import { FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { CardItem, Priority } from '../../models/card.models';
import { ListItem } from '../../models/list-model';
import { ToastService } from '../../services/toast.service/toast.service';
import { checkErrors } from '../../services/form-validator';

@Component({
  selector: 'app-card-modal',
  imports: [ReactiveFormsModule],
  templateUrl: './card-modal.html',
  styleUrl: './card-modal.css',
})
export class CardModal implements OnChanges {
  @Input() cardData?: CardItem;
  @Input() currentListId?: string;
  @Input() lists?: ListItem[];
  @Output() isOpened = new EventEmitter<boolean>();
  @Output() savedCard = new EventEmitter<Partial<CardItem>>();

  private toaster = inject(ToastService);

  cardForm = new FormGroup({
    title: new FormControl('', [Validators.required, Validators.maxLength(50)]),
    listId: new FormControl('', [Validators.required]),
    description: new FormControl(''),
    dueDate: new FormControl('', [Validators.required]),
    priority: new FormControl('', [Validators.required]),
  });
  
  ngOnChanges(changes: SimpleChanges) {
    if (this.cardData) {
      const formattedDate = this.cardData.dueDate
        ? this.cardData.dueDate.toString().split('T')[0]
        : '';

      this.cardForm.patchValue({
        title: this.cardData.title,
        listId: this.cardData.listId,
        description: this.cardData.description,
        priority: this.cardData.priority,
        dueDate: formattedDate,
      });
    } 
    else if (this.currentListId) {
      this.cardForm.reset();
      this.cardForm.patchValue({ 
        priority: Priority.Low,
        listId: this.currentListId
      });
    }
  }

  onSave() {
    const dictionary = {
      title: 'Title',
      listId: 'List',
      description: 'Description',
      dueDate: 'Due Date',
      priority: 'Priority',
    };

    const isValid = checkErrors(this.cardForm, dictionary);

    if (isValid === null) {
      this.savedCard.emit(this.cardForm.value as Partial<CardItem>);
    } else {
      this.toaster.warning(isValid);
    }
  }

  close() {
    this.cardForm.reset();

    this.isOpened.emit(false);
  }
}
