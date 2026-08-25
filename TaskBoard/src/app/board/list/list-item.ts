import { CardItem } from "./card/card-item";

export interface ListItem {
    id: string;
    title: string;
    position: number;
    cards: CardItem[];
}