import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';

import { Observable } from 'rxjs';

import { BasketService } from './basket.service';
import { IBasket } from '../shared/models/basket.model';
import { IBasketItem } from '../shared/models/basketItem.model';
import { BasketWrapperService } from '../shared/services/basket.wrapper.service';

@Component({
    selector: 'f-basket',
    styleUrls: ['./basket.component.scss'],
    templateUrl: './basket.component.html'
})
export class BasketComponent implements OnInit {
    errorMessages: any;
    basket: IBasket;
    totalAmount: number = 0;

    constructor(private service: BasketService, private router: Router, private basketwrapper: BasketWrapperService) { }

    ngOnInit() {
        this.service.getBasket().subscribe(basket => {
            this.basket = basket;
            this.calculateTotalAmount();
        });
    }

    itemQuantityChanged(item: IBasketItem) {
        this.calculateTotalAmount();
        this.service.setBasket(this.basket).subscribe(x => console.log('basket updated: ' + x));
    }

    update(event: any): Observable<boolean> {
        let setBasketObservable = this.service.setBasket(this.basket);
        setBasketObservable
            .subscribe(
            x => {
                this.errorMessages = [];
                console.log('basket updated: ' + x);
            },
            errMessage => this.errorMessages = errMessage.messages);
        return setBasketObservable;
    }

    checkOut(event: any) {
        this.update(event)
            .subscribe(
                x => {
                    this.errorMessages = [];
                    this.basketwrapper.basket = this.basket;
                    this.router.navigate(['application']);
        });
    }

    private calculateTotalAmount() {
        this.totalAmount = 0;
        this.basket.items.forEach(item => {
            this.totalAmount += (item.slotAmount * item.slots);
        });
    }
}