export interface IScholarshipItem {
    id: number;
    name: string;
    description: string;
    amount: number;
    pictureUri: string;
    scholarshipLocationId: number;
    scholarshipLocation: string;
    scholarshipEducationLevelId: number;
    scholarshipEducationLevel: string;
    slots: number;
}