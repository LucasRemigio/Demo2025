import { Department } from 'app/core/user/user.types';

export interface AccountManagmentEntry {
    email: string;
    name: string;
    password: string;
    active_since: string;
    last_login: string;
    role_description: string;
    roles: string[];
}

export interface AccountEntry {
    email: string;
    name: string;
    password: string;
    user_role_id: number;
    departments: Department[];
}

export interface UpdateAccountEntry {
    updated_name: string;
    email: string;
    updated_password: string;
    updated_departments: Department[];
}

export interface DepartmentRoleEntry {
    department: Department;
    checked?: boolean;
    disabled?: boolean; //admins cannot have their permissions changed
}
