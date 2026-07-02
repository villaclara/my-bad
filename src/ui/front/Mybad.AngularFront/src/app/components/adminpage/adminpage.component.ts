import { NgFor, NgIf, } from '@angular/common';
import { Component, inject, OnInit, signal } from '@angular/core';
import { AdminService } from '../../services/admin.service';
import { Pagination, TableColumnMeta, TableDataApiResponse, TableMetaData } from '../../models/tables';

interface ColumnDef {
  name: string;
  type: string;
  isKey?: boolean;
  isNumber?: boolean;
}

@Component({
  selector: 'app-adminpage',
  standalone: true,
  imports: [NgFor, NgIf],
  templateUrl: './adminpage.component.html',
  styleUrl: './adminpage.component.css'
})
export class AdminpageComponent {
  isKeySet = signal<boolean>(false);
  isMobileMenuOpen = false;
  activeTable = signal('');
  errorMsg: string | null = null;

  private adminService = inject(AdminService);

  tables = signal<TableMetaData[]>([]);
  columns = signal<TableColumnMeta[]>([]);
  tableData = signal<any[]>([]);
  pagination = signal<Pagination>({
    page: 1,
    pageSize: 50,
    totalCount: 0,
    totalPages: 1
  });

  initTablesMetadata(): void {
    this.adminService.getTablesMetaData().subscribe({
      next: (data: TableMetaData[]) => {
        this.tables.set(data);
        this.activeTable.set(this.tables()[0].name);
        this.columns.set(this.tables().find(x => x.name === this.activeTable())?.columns ?? []);
        this.loadTable(this.activeTable(), 1, 100);
        this.isKeySet.set(true);
      }
    });
  }

  loadTable(tableName: string, targetPage: number, targetSize: number) {
    this.activeTable.set(tableName);
    this.columns.set(this.tables().find(x => x.name === this.activeTable())?.columns ?? []);
    this.adminService.getTableData(this.activeTable(), targetPage, targetSize).subscribe({
      next: (data: TableDataApiResponse) => {
        this.tableData.set(data.data);
        this.pagination.set(data.meta);
        this.isKeySet.set(true);
      },
      error: () => {
        this.errorMsg = 'Could not load table data of table ' + this.activeTable;
        this.isKeySet.set(false);
      }
    })
  }

  goToPage(targetPage: number) {
    const meta = this.pagination();
    if (targetPage >= 1 && targetPage <= meta.totalPages && targetPage !== meta.page) {
      this.loadTable(this.activeTable(), targetPage, meta.pageSize);
    }
  }

  onPageInput(event: Event) {
    const input = event.target as HTMLInputElement;
    let targetPage = parseInt(input.value, 10);
    const meta = this.pagination();

    if (isNaN(targetPage) || targetPage < 1) {
      targetPage = 1;
    } else if (targetPage > meta.totalPages) {
      targetPage = meta.totalPages;
    }

    input.value = targetPage.toString(); // Sync input UI value
    if (targetPage !== meta.page) {
      this.loadTable(this.activeTable(), targetPage, meta.pageSize);
    }
  }

  onPageSizeChange(event: Event) {
    const select = event.target as HTMLSelectElement;
    const newSize = parseInt(select.value, 10);
    this.loadTable(this.activeTable(), 1, newSize);
  }

  getPascalValue(row: any, colName: string): any {
    const actualKey = Object.keys(row).find(
      key => key.toLowerCase() === colName.toLowerCase()
    );
    return actualKey ? row[actualKey] : '';
  }

  setKey(key: string): void {
    this.adminService.setKey(key);
    this.initTablesMetadata();
  }
}
