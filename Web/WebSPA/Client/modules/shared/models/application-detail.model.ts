import { IApplicationItem} from './applicationItem.model';

export interface IApplicationDetail {
    applicationnumber: string;
    status: string;
    description: string;
    idnumber: string;
    request: number;
    date: Date;
    total: number;
    applicationitems: IApplicationItem[];
}