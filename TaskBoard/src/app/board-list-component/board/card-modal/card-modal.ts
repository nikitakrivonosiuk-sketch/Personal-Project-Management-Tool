import { Component, EventEmitter, inject, Input, OnChanges, Output, SimpleChanges } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { CardItem } from '../list/card/card-item';
import { ListItem } from '../list/list-item';
import { Store } from '@ngrx/store';
import { showToast } from '../../../toast.service/toast.action';
import { ToastType } from '../../../toast.service/toast.reducer';
import { CardValidator } from './card.validator';
import { ToastService } from '../../../toast.service/toast.service';

@Component({
  selector: 'app-card-modal',
  imports: [FormsModule],
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

  cardModel: Partial<CardItem> = {
    title: '',
    listId: '',
    description: '',
    priority: 'Low',
  };

  ngOnChanges(changes: SimpleChanges){
    if (this.cardData){
      this.cardModel = {...this.cardData}

      if (this.cardModel.dueDate) {
      this.cardModel.dueDate = this.cardModel.dueDate.toString().split('T')[0];
    }
    }
    else if(this.currentListId) {
      this.cardModel = {
        title: '',
        description: '',
        priority: 'Low',
        listId: this.currentListId
      };
    }
  }

  onSave(){
    const validated = CardValidator.validate(this.cardModel);

    if (validated === null){
      this.savedCard.emit(this.cardModel);
      this.toaster.success('Card was saved succesfully!');
    }
    else {
      this.toaster.warning(validated);
    }
  }

  close(){
    this.cardModel.description = '';

    this.isOpened.emit(false);
  }
}
