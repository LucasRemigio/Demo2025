import { Component, Input, OnChanges, OnInit } from '@angular/core';
import { CurrentMapsAddress } from '../confirm-order-address.types';

@Component({
    selector: 'app-distance-info',
    templateUrl: './distance-info.component.html',
    styleUrls: ['./distance-info.component.scss'],
})
export class DistanceInfoComponent implements OnInit, OnChanges {
    @Input() distanceMatrix: CurrentMapsAddress;

    distanceKms: string;
    travelTimeMins: string;

    constructor() {}

    ngOnInit(): void {
        this.formatData(this.distanceMatrix);
    }

    ngOnChanges(): void {
        this.formatData(this.distanceMatrix);
    }

    formatData(destinationDetails: CurrentMapsAddress): void {
        if (!destinationDetails) {
            return;
        }

        // the distance is currently in meters, need to format it to kms
        const distanceKms = (this.distanceMatrix.distance / 1000).toFixed(2);
        this.distanceKms = `${distanceKms} km`;

        // the traveltime is currently in seconds, need to format it to mins
        const travelTimeMins = Math.ceil(this.distanceMatrix.travel_time / 60);
        this.travelTimeMins = `${travelTimeMins} min`;
    }
}
