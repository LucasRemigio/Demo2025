import {
    ChangeDetectorRef,
    Component,
    EventEmitter,
    Input,
    OnInit,
    Output,
} from '@angular/core';
import { DepartmentRoleEntry } from '../accounts-managment.types';
import { AccountsManagmentService } from '../accounts-managment.service';
import { FuseSplashScreenService } from '@fuse/services/splash-screen';
import { Department } from 'app/core/user/user.types';

@Component({
    selector: 'app-role-manager',
    templateUrl: './role-manager.component.html',
    styleUrls: ['./role-manager.component.scss'],
})
export class RoleManagerComponent implements OnInit {
    @Input() userEmail?: string;

    // checkbox that reflects the roles of a user
    options: DepartmentRoleEntry[] = [];

    constructor(
        private _accountMngService: AccountsManagmentService,
        private cdr: ChangeDetectorRef,
        private _fuseSplashScreenService: FuseSplashScreenService
    ) {}

    ngOnInit(): void {
        // Initialize the options checkbox with proper values, and lock them until fully loaded
        this._accountMngService.getDepartments().subscribe(async (result) => {
            let tempOptions = [];
            if (result) {
                Object.entries(result.departments).forEach(([index, value]) => {
                    tempOptions.push({
                        department: value,
                        checked: false,
                        disabled: true,
                    });
                });
            }
            this.options = tempOptions;
            if (this.userEmail) {
                await this.userEmailSelect(this.userEmail);
            }

            this.unlockOptions(); // after everything loaded, unlock the checkboxes
            this.cdr.detectChanges(); // Manually trigger change detection
        });
    }

    ngOnDestroy(): void {}

    getDepartments(): Department[] {
        const departments: Department[] = [];
        this.options.forEach((entry) => {
            if (entry.checked) {
                departments.push(entry.department);
            }
        });
        return departments;
    }

    getOptions() {
        return this.options;
    }

    lockOptions() {
        this.options.forEach((option) => {
            option.disabled = true;
        });
    }

    unlockOptions() {
        this.options.forEach((option) => {
            option.disabled = false;
        });
    }

    updateOption(id: string, checked: boolean): void {
        this.options[id].checked = checked;
        this.cdr.detectChanges();
    }

    // Updates options according to array of departments
    updateAllOptions(departments?: string[]) {
        if (departments) {
            this.options.forEach((option) => {
                if (departments.includes(option.department.name)) {
                    option.checked = true;
                } else {
                    option.checked = false;
                }
            });
        }
    }

    // All options true and lock all
    adminSelect() {
        this.options.forEach((option) => {
            option.disabled = true;
            option.checked = true;
        });
        this.updateAllOptions();
    }

    // All options false and unlock all
    userSelect() {
        this.options.forEach((option) => {
            option.disabled = false;
            option.checked = false;
        });
        this.cdr.detectChanges(); // Manually trigger change detection
    }

    // if email is provided, fill options with user departments
    async userEmailSelect(email: string): Promise<void> {
        this.options.forEach((option) => {
            option.checked = false;
        });

        // get user's departments
        try {
            const result = await this._accountMngService
                .getUserDepartments(email)
                .toPromise();
            let departments = [];

            if (Array.isArray(result.departments)) {
                result.departments.forEach((value) => {
                    departments.push(value.name.toString());
                });
            }

            this.updateAllOptions(departments);
        } catch (error) {
            console.error(error);
        }
    }
}
