import { CardItem } from '../list/card/card-item';

export class CardValidator {
  public static validate(card: Partial<CardItem>) {
    if (!card.title?.trim()) {
      return 'Please enter the card name.';
    }
    if (!card.dueDate) {
      return 'Please enter the date.';
    }
    if (!card.priority) {
      return 'Please enter the card priority.';
    }
    if (card.listId === '') {
      return 'Please choose the list.';
    }
    return null;
  }
}
