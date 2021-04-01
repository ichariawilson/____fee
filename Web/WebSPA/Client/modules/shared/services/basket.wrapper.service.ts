import { Injectable }       from '@angular/core';
import { Subject }          from 'rxjs';

import { IScholarshipItem }     from '../models/scholarshipItem.model';
import { IBasketItem }      from '../models/basketItem.model';
import { IBasket }          from '../models/basket.model';
import { SecurityService } from '../services/security.service';
import { Guid } from '../../../guid';

@Injectable()
export class BasketWrapperService {
    public basket: IBasket;

    constructor(private identityService: SecurityService) { }

    // observable that is fired when a scholarship item is added to the cart
    private addItemToBasketSource = new Subject<IBasketItem>();
    addItemToBasket$ = this.addItemToBasketSource.asObservable();

    private applicationCreatedSource = new Subject();
    applicationCreated$ = this.applicationCreatedSource.asObservable();

    addItemToBasket(item: IScholarshipItem) {
        if (this.identityService.IsAuthorized) {
            let basket: IBasketItem = {
                pictureUrl: item.pictureUri,
                scholarshipitemId: item.id,
                scholarshipitemName: item.name,
                slots: 1,
                slotAmount: item.amount,
                id: Guid.newGuid(),
                oldslotAmount: 0
            };

            this.addItemToBasketSource.next(basket);
        } else {
            this.identityService.Authorize();
        }
    }

    applicationCreated() {
        this.applicationCreatedSource.next();
    }
}