import { Injectable } from '@angular/core';
import { HttpResponse } from '@angular/common/http';
import { Router } from '@angular/router';

import { DataService } from '../shared/services/data.service';
import { SecurityService } from '../shared/services/security.service';
import { IBasket } from '../shared/models/basket.model';
import { IApplication } from '../shared/models/application.model';
import { IBasketCheckout } from '../shared/models/basketCheckout.model';
import { BasketWrapperService } from '../shared/services/basket.wrapper.service';
import { ConfigurationService } from '../shared/services/configuration.service';
import { StorageService } from '../shared/services/storage.service';

import { Observable, Observer, Subject } from 'rxjs';
import { map, catchError, tap } from 'rxjs/operators';

@Injectable()
export class BasketService {
    private applyingbasketUrl: string = '';
    private applyUrl: string = '';
    basket: IBasket = {
        studentId: '',
        items: []
    };

    //observable that is fired when the basket is dropped
    private basketDropedSource = new Subject();
    basketDroped$ = this.basketDropedSource.asObservable();

    constructor(private service: DataService, private authService: SecurityService, private basketEvents: BasketWrapperService, private router: Router, private configurationService: ConfigurationService, private storageService: StorageService) {
        this.basket.items = [];

        // Init:
        if (this.authService.IsAuthorized) {
            if (this.authService.UserData) {
                this.basket.studentId = this.authService.UserData.sub;
                if (this.configurationService.isReady) {
                    this.applyingbasketUrl = this.configurationService.serverSettings.applyUrl;
                    this.applyUrl = this.configurationService.serverSettings.applyUrl;
                    this.loadData();
                }
                else {
                    this.configurationService.settingsLoaded$.subscribe(x => {
                        this.applyingbasketUrl = this.configurationService.serverSettings.applyUrl;
                        this.applyUrl = this.configurationService.serverSettings.applyUrl;
                        this.loadData();
                    });
                }
            }
        }

        this.basketEvents.applicationCreated$.subscribe(x => {
            this.dropBasket();
        });
    }

    addItemToBasket(item): Observable<boolean> {
        this.basket.items.push(item);
        return this.setBasket(this.basket);
    }

    setBasket(basket): Observable<boolean> {
        let url = this.applyUrl + '/api/v1/basket/';

        this.basket = basket;

        return this.service.post(url, basket).pipe<boolean>(tap((response: any) => true));
    }

    setBasketCheckout(basketCheckout): Observable<boolean> {
        let url = this.applyingbasketUrl + '/b/api/v1/basket/checkout';

        return this.service.postWithId(url, basketCheckout).pipe<boolean>(tap((response: any) => {
            this.basketEvents.applicationCreated();
            return true;
        }));
    }

    getBasket(): Observable<IBasket> {
        let url = this.applyingbasketUrl + '/b/api/v1/basket/' + this.basket.studentId;

        return this.service.get(url).pipe<IBasket>(tap((response: any) => {
            if (response.status === 204) {
                return null;
            }

            return response;
        }));
    }

    mapBasketInfoCheckout(application: IApplication): IBasketCheckout {
        let basketCheckout = <IBasketCheckout>{};

        basketCheckout.idnumber = application.idnumber;
        basketCheckout.request = application.request;
        basketCheckout.paymenttypeid = application.paymenttypeid;
        basketCheckout.total = 0;

        return basketCheckout;
    }

    dropBasket() {
        this.basket.items = [];
        this.basketDropedSource.next();
    }

    private loadData() {
        this.getBasket().subscribe(basket => {
            if (basket != null)
                this.basket.items = basket.items;
        });
    }
}