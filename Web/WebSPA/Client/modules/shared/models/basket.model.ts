import { IBasketItem } from './basketItem.model';

export interface IBasket {
    items: IBasketItem[];
    studentId: string;
}
