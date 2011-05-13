﻿#pragma warning disable 1591
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:2.0.50727.4016
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace TSDTReports.Models
{
	using System.Data.Linq;
	using System.Data.Linq.Mapping;
	using System.Data;
	using System.Collections.Generic;
	using System.Reflection;
	using System.Linq;
	using System.Linq.Expressions;
	using System.ComponentModel;
	using System;
	
	
	[System.Data.Linq.Mapping.DatabaseAttribute(Name="SacoEnterprise")]
	public partial class sacoDataContext : System.Data.Linq.DataContext
	{
		
		private static System.Data.Linq.Mapping.MappingSource mappingSource = new AttributeMappingSource();
		
    #region Extensibility Method Definitions
    partial void OnCreated();
    partial void InsertCompany(Company instance);
    partial void UpdateCompany(Company instance);
    partial void DeleteCompany(Company instance);
    partial void InsertEmployee(Employee instance);
    partial void UpdateEmployee(Employee instance);
    partial void DeleteEmployee(Employee instance);
    partial void InsertReader(Reader instance);
    partial void UpdateReader(Reader instance);
    partial void DeleteReader(Reader instance);
    #endregion
		
		public sacoDataContext() : 
				base(global::System.Configuration.ConfigurationManager.ConnectionStrings["SacoEnterpriseConnectionString"].ConnectionString, mappingSource)
		{
			OnCreated();
		}
		
		public sacoDataContext(string connection) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public sacoDataContext(System.Data.IDbConnection connection) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public sacoDataContext(string connection, System.Data.Linq.Mapping.MappingSource mappingSource) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public sacoDataContext(System.Data.IDbConnection connection, System.Data.Linq.Mapping.MappingSource mappingSource) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public System.Data.Linq.Table<Company> Companies
		{
			get
			{
				return this.GetTable<Company>();
			}
		}
		
		public System.Data.Linq.Table<Employee> Employees
		{
			get
			{
				return this.GetTable<Employee>();
			}
		}
		
		public System.Data.Linq.Table<ClockHistory> ClockHistories
		{
			get
			{
				return this.GetTable<ClockHistory>();
			}
		}
		
		public System.Data.Linq.Table<Reader> Readers
		{
			get
			{
				return this.GetTable<Reader>();
			}
		}
	}
	
	[Table(Name="dbo.Company")]
	public partial class Company : INotifyPropertyChanging, INotifyPropertyChanged
	{
		
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
		private string _CompanyId;
		
		private char _Status;
		
		private string _Name;
		
		private System.Nullable<char> _RiskCompany;
		
		private EntitySet<Employee> _Employees;
		
    #region Extensibility Method Definitions
    partial void OnLoaded();
    partial void OnValidate(System.Data.Linq.ChangeAction action);
    partial void OnCreated();
    partial void OnCompanyIdChanging(string value);
    partial void OnCompanyIdChanged();
    partial void OnStatusChanging(char value);
    partial void OnStatusChanged();
    partial void OnNameChanging(string value);
    partial void OnNameChanged();
    partial void OnRiskCompanyChanging(System.Nullable<char> value);
    partial void OnRiskCompanyChanged();
    #endregion
		
		public Company()
		{
			this._Employees = new EntitySet<Employee>(new Action<Employee>(this.attach_Employees), new Action<Employee>(this.detach_Employees));
			OnCreated();
		}
		
		[Column(Storage="_CompanyId", DbType="VarChar(20) NOT NULL", CanBeNull=false, IsPrimaryKey=true)]
		public string CompanyId
		{
			get
			{
				return this._CompanyId;
			}
			set
			{
				if ((this._CompanyId != value))
				{
					this.OnCompanyIdChanging(value);
					this.SendPropertyChanging();
					this._CompanyId = value;
					this.SendPropertyChanged("CompanyId");
					this.OnCompanyIdChanged();
				}
			}
		}
		
		[Column(Storage="_Status", DbType="Char(1) NOT NULL")]
		public char Status
		{
			get
			{
				return this._Status;
			}
			set
			{
				if ((this._Status != value))
				{
					this.OnStatusChanging(value);
					this.SendPropertyChanging();
					this._Status = value;
					this.SendPropertyChanged("Status");
					this.OnStatusChanged();
				}
			}
		}
		
		[Column(Storage="_Name", DbType="VarChar(30) NOT NULL", CanBeNull=false)]
		public string Name
		{
			get
			{
				return this._Name;
			}
			set
			{
				if ((this._Name != value))
				{
					this.OnNameChanging(value);
					this.SendPropertyChanging();
					this._Name = value;
					this.SendPropertyChanged("Name");
					this.OnNameChanged();
				}
			}
		}
		
		[Column(Storage="_RiskCompany", DbType="Char(1)")]
		public System.Nullable<char> RiskCompany
		{
			get
			{
				return this._RiskCompany;
			}
			set
			{
				if ((this._RiskCompany != value))
				{
					this.OnRiskCompanyChanging(value);
					this.SendPropertyChanging();
					this._RiskCompany = value;
					this.SendPropertyChanged("RiskCompany");
					this.OnRiskCompanyChanged();
				}
			}
		}
		
		[Association(Name="Company_Employee", Storage="_Employees", ThisKey="CompanyId", OtherKey="CompanyId")]
		public EntitySet<Employee> Employees
		{
			get
			{
				return this._Employees;
			}
			set
			{
				this._Employees.Assign(value);
			}
		}
		
		public event PropertyChangingEventHandler PropertyChanging;
		
		public event PropertyChangedEventHandler PropertyChanged;
		
		protected virtual void SendPropertyChanging()
		{
			if ((this.PropertyChanging != null))
			{
				this.PropertyChanging(this, emptyChangingEventArgs);
			}
		}
		
		protected virtual void SendPropertyChanged(String propertyName)
		{
			if ((this.PropertyChanged != null))
			{
				this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}
		
		private void attach_Employees(Employee entity)
		{
			this.SendPropertyChanging();
			entity.Company = this;
		}
		
		private void detach_Employees(Employee entity)
		{
			this.SendPropertyChanging();
			entity.Company = null;
		}
	}
	
	[Table(Name="dbo.Employee")]
	public partial class Employee : INotifyPropertyChanging, INotifyPropertyChanged
	{
		
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
		private string _EmployeeNumber;
		
		private int _BadgeHolderNumber;
		
		private string _Surname;
		
		private string _Initials;
		
		private string _FirstName;
		
		private string _IdNumber;
		
		private string _ShiftCycleId;
		
		private System.DateTime _CycleDate;
		
		private short _DayInCycle;
		
		private string _Department;
		
		private string _PayrollId;
		
		private string _OccupationId;
		
		private System.DateTime _EngagementDate;
		
		private string _IndustryNumber;
		
		private string _EmployeeCategoryId;
		
		private string _WorkAreaId;
		
		private string _MedicalClassId;
		
		private char _Contractor;
		
		private string _CompanyId;
		
		private char _Callout;
		
		private short _VisitorCount;
		
		private char _OnShift;
		
		private System.Nullable<System.DateTime> _LastMedicalCheckDate;
		
		private System.Nullable<double> _Balance;
		
		private EntityRef<Company> _Company;
		
    #region Extensibility Method Definitions
    partial void OnLoaded();
    partial void OnValidate(System.Data.Linq.ChangeAction action);
    partial void OnCreated();
    partial void OnEmployeeNumberChanging(string value);
    partial void OnEmployeeNumberChanged();
    partial void OnBadgeHolderNumberChanging(int value);
    partial void OnBadgeHolderNumberChanged();
    partial void OnSurnameChanging(string value);
    partial void OnSurnameChanged();
    partial void OnInitialsChanging(string value);
    partial void OnInitialsChanged();
    partial void OnFirstNameChanging(string value);
    partial void OnFirstNameChanged();
    partial void OnIdNumberChanging(string value);
    partial void OnIdNumberChanged();
    partial void OnShiftCycleIdChanging(string value);
    partial void OnShiftCycleIdChanged();
    partial void OnCycleDateChanging(System.DateTime value);
    partial void OnCycleDateChanged();
    partial void OnDayInCycleChanging(short value);
    partial void OnDayInCycleChanged();
    partial void OnDepartmentChanging(string value);
    partial void OnDepartmentChanged();
    partial void OnPayrollIdChanging(string value);
    partial void OnPayrollIdChanged();
    partial void OnOccupationIdChanging(string value);
    partial void OnOccupationIdChanged();
    partial void OnEngagementDateChanging(System.DateTime value);
    partial void OnEngagementDateChanged();
    partial void OnIndustryNumberChanging(string value);
    partial void OnIndustryNumberChanged();
    partial void OnEmployeeCategoryIdChanging(string value);
    partial void OnEmployeeCategoryIdChanged();
    partial void OnWorkAreaIdChanging(string value);
    partial void OnWorkAreaIdChanged();
    partial void OnMedicalClassIdChanging(string value);
    partial void OnMedicalClassIdChanged();
    partial void OnContractorChanging(char value);
    partial void OnContractorChanged();
    partial void OnCompanyIdChanging(string value);
    partial void OnCompanyIdChanged();
    partial void OnCalloutChanging(char value);
    partial void OnCalloutChanged();
    partial void OnVisitorCountChanging(short value);
    partial void OnVisitorCountChanged();
    partial void OnOnShiftChanging(char value);
    partial void OnOnShiftChanged();
    partial void OnLastMedicalCheckDateChanging(System.Nullable<System.DateTime> value);
    partial void OnLastMedicalCheckDateChanged();
    partial void OnBalanceChanging(System.Nullable<double> value);
    partial void OnBalanceChanged();
    #endregion
		
		public Employee()
		{
			this._Company = default(EntityRef<Company>);
			OnCreated();
		}
		
		[Column(Storage="_EmployeeNumber", DbType="VarChar(20) NOT NULL", CanBeNull=false, IsPrimaryKey=true)]
		public string EmployeeNumber
		{
			get
			{
				return this._EmployeeNumber;
			}
			set
			{
				if ((this._EmployeeNumber != value))
				{
					this.OnEmployeeNumberChanging(value);
					this.SendPropertyChanging();
					this._EmployeeNumber = value;
					this.SendPropertyChanged("EmployeeNumber");
					this.OnEmployeeNumberChanged();
				}
			}
		}
		
		[Column(Storage="_BadgeHolderNumber", DbType="Int NOT NULL")]
		public int BadgeHolderNumber
		{
			get
			{
				return this._BadgeHolderNumber;
			}
			set
			{
				if ((this._BadgeHolderNumber != value))
				{
					this.OnBadgeHolderNumberChanging(value);
					this.SendPropertyChanging();
					this._BadgeHolderNumber = value;
					this.SendPropertyChanged("BadgeHolderNumber");
					this.OnBadgeHolderNumberChanged();
				}
			}
		}
		
		[Column(Storage="_Surname", DbType="VarChar(30) NOT NULL", CanBeNull=false)]
		public string Surname
		{
			get
			{
				return this._Surname;
			}
			set
			{
				if ((this._Surname != value))
				{
					this.OnSurnameChanging(value);
					this.SendPropertyChanging();
					this._Surname = value;
					this.SendPropertyChanged("Surname");
					this.OnSurnameChanged();
				}
			}
		}
		
		[Column(Storage="_Initials", DbType="VarChar(6) NOT NULL", CanBeNull=false)]
		public string Initials
		{
			get
			{
				return this._Initials;
			}
			set
			{
				if ((this._Initials != value))
				{
					this.OnInitialsChanging(value);
					this.SendPropertyChanging();
					this._Initials = value;
					this.SendPropertyChanged("Initials");
					this.OnInitialsChanged();
				}
			}
		}
		
		[Column(Storage="_FirstName", DbType="VarChar(20) NOT NULL", CanBeNull=false)]
		public string FirstName
		{
			get
			{
				return this._FirstName;
			}
			set
			{
				if ((this._FirstName != value))
				{
					this.OnFirstNameChanging(value);
					this.SendPropertyChanging();
					this._FirstName = value;
					this.SendPropertyChanged("FirstName");
					this.OnFirstNameChanged();
				}
			}
		}
		
		[Column(Storage="_IdNumber", DbType="VarChar(20)")]
		public string IdNumber
		{
			get
			{
				return this._IdNumber;
			}
			set
			{
				if ((this._IdNumber != value))
				{
					this.OnIdNumberChanging(value);
					this.SendPropertyChanging();
					this._IdNumber = value;
					this.SendPropertyChanged("IdNumber");
					this.OnIdNumberChanged();
				}
			}
		}
		
		[Column(Storage="_ShiftCycleId", DbType="Char(6) NOT NULL", CanBeNull=false)]
		public string ShiftCycleId
		{
			get
			{
				return this._ShiftCycleId;
			}
			set
			{
				if ((this._ShiftCycleId != value))
				{
					this.OnShiftCycleIdChanging(value);
					this.SendPropertyChanging();
					this._ShiftCycleId = value;
					this.SendPropertyChanged("ShiftCycleId");
					this.OnShiftCycleIdChanged();
				}
			}
		}
		
		[Column(Storage="_CycleDate", DbType="DateTime NOT NULL")]
		public System.DateTime CycleDate
		{
			get
			{
				return this._CycleDate;
			}
			set
			{
				if ((this._CycleDate != value))
				{
					this.OnCycleDateChanging(value);
					this.SendPropertyChanging();
					this._CycleDate = value;
					this.SendPropertyChanged("CycleDate");
					this.OnCycleDateChanged();
				}
			}
		}
		
		[Column(Storage="_DayInCycle", DbType="SmallInt NOT NULL")]
		public short DayInCycle
		{
			get
			{
				return this._DayInCycle;
			}
			set
			{
				if ((this._DayInCycle != value))
				{
					this.OnDayInCycleChanging(value);
					this.SendPropertyChanging();
					this._DayInCycle = value;
					this.SendPropertyChanged("DayInCycle");
					this.OnDayInCycleChanged();
				}
			}
		}
		
		[Column(Storage="_Department", DbType="Char(12) NOT NULL", CanBeNull=false)]
		public string Department
		{
			get
			{
				return this._Department;
			}
			set
			{
				if ((this._Department != value))
				{
					this.OnDepartmentChanging(value);
					this.SendPropertyChanging();
					this._Department = value;
					this.SendPropertyChanged("Department");
					this.OnDepartmentChanged();
				}
			}
		}
		
		[Column(Storage="_PayrollId", DbType="Char(6) NOT NULL", CanBeNull=false)]
		public string PayrollId
		{
			get
			{
				return this._PayrollId;
			}
			set
			{
				if ((this._PayrollId != value))
				{
					this.OnPayrollIdChanging(value);
					this.SendPropertyChanging();
					this._PayrollId = value;
					this.SendPropertyChanged("PayrollId");
					this.OnPayrollIdChanged();
				}
			}
		}
		
		[Column(Storage="_OccupationId", DbType="VarChar(12) NOT NULL", CanBeNull=false)]
		public string OccupationId
		{
			get
			{
				return this._OccupationId;
			}
			set
			{
				if ((this._OccupationId != value))
				{
					this.OnOccupationIdChanging(value);
					this.SendPropertyChanging();
					this._OccupationId = value;
					this.SendPropertyChanged("OccupationId");
					this.OnOccupationIdChanged();
				}
			}
		}
		
		[Column(Storage="_EngagementDate", DbType="DateTime NOT NULL")]
		public System.DateTime EngagementDate
		{
			get
			{
				return this._EngagementDate;
			}
			set
			{
				if ((this._EngagementDate != value))
				{
					this.OnEngagementDateChanging(value);
					this.SendPropertyChanging();
					this._EngagementDate = value;
					this.SendPropertyChanged("EngagementDate");
					this.OnEngagementDateChanged();
				}
			}
		}
		
		[Column(Storage="_IndustryNumber", DbType="Char(8)")]
		public string IndustryNumber
		{
			get
			{
				return this._IndustryNumber;
			}
			set
			{
				if ((this._IndustryNumber != value))
				{
					this.OnIndustryNumberChanging(value);
					this.SendPropertyChanging();
					this._IndustryNumber = value;
					this.SendPropertyChanged("IndustryNumber");
					this.OnIndustryNumberChanged();
				}
			}
		}
		
		[Column(Storage="_EmployeeCategoryId", DbType="Char(6) NOT NULL", CanBeNull=false)]
		public string EmployeeCategoryId
		{
			get
			{
				return this._EmployeeCategoryId;
			}
			set
			{
				if ((this._EmployeeCategoryId != value))
				{
					this.OnEmployeeCategoryIdChanging(value);
					this.SendPropertyChanging();
					this._EmployeeCategoryId = value;
					this.SendPropertyChanged("EmployeeCategoryId");
					this.OnEmployeeCategoryIdChanged();
				}
			}
		}
		
		[Column(Storage="_WorkAreaId", DbType="Char(6) NOT NULL", CanBeNull=false)]
		public string WorkAreaId
		{
			get
			{
				return this._WorkAreaId;
			}
			set
			{
				if ((this._WorkAreaId != value))
				{
					this.OnWorkAreaIdChanging(value);
					this.SendPropertyChanging();
					this._WorkAreaId = value;
					this.SendPropertyChanged("WorkAreaId");
					this.OnWorkAreaIdChanged();
				}
			}
		}
		
		[Column(Storage="_MedicalClassId", DbType="Char(6)")]
		public string MedicalClassId
		{
			get
			{
				return this._MedicalClassId;
			}
			set
			{
				if ((this._MedicalClassId != value))
				{
					this.OnMedicalClassIdChanging(value);
					this.SendPropertyChanging();
					this._MedicalClassId = value;
					this.SendPropertyChanged("MedicalClassId");
					this.OnMedicalClassIdChanged();
				}
			}
		}
		
		[Column(Storage="_Contractor", DbType="Char(1) NOT NULL")]
		public char Contractor
		{
			get
			{
				return this._Contractor;
			}
			set
			{
				if ((this._Contractor != value))
				{
					this.OnContractorChanging(value);
					this.SendPropertyChanging();
					this._Contractor = value;
					this.SendPropertyChanged("Contractor");
					this.OnContractorChanged();
				}
			}
		}
		
		[Column(Storage="_CompanyId", DbType="VarChar(20)")]
		public string CompanyId
		{
			get
			{
				return this._CompanyId;
			}
			set
			{
				if ((this._CompanyId != value))
				{
					if (this._Company.HasLoadedOrAssignedValue)
					{
						throw new System.Data.Linq.ForeignKeyReferenceAlreadyHasValueException();
					}
					this.OnCompanyIdChanging(value);
					this.SendPropertyChanging();
					this._CompanyId = value;
					this.SendPropertyChanged("CompanyId");
					this.OnCompanyIdChanged();
				}
			}
		}
		
		[Column(Storage="_Callout", DbType="Char(1) NOT NULL")]
		public char Callout
		{
			get
			{
				return this._Callout;
			}
			set
			{
				if ((this._Callout != value))
				{
					this.OnCalloutChanging(value);
					this.SendPropertyChanging();
					this._Callout = value;
					this.SendPropertyChanged("Callout");
					this.OnCalloutChanged();
				}
			}
		}
		
		[Column(Storage="_VisitorCount", DbType="SmallInt NOT NULL")]
		public short VisitorCount
		{
			get
			{
				return this._VisitorCount;
			}
			set
			{
				if ((this._VisitorCount != value))
				{
					this.OnVisitorCountChanging(value);
					this.SendPropertyChanging();
					this._VisitorCount = value;
					this.SendPropertyChanged("VisitorCount");
					this.OnVisitorCountChanged();
				}
			}
		}
		
		[Column(Storage="_OnShift", DbType="Char(1) NOT NULL")]
		public char OnShift
		{
			get
			{
				return this._OnShift;
			}
			set
			{
				if ((this._OnShift != value))
				{
					this.OnOnShiftChanging(value);
					this.SendPropertyChanging();
					this._OnShift = value;
					this.SendPropertyChanged("OnShift");
					this.OnOnShiftChanged();
				}
			}
		}
		
		[Column(Storage="_LastMedicalCheckDate", DbType="DateTime")]
		public System.Nullable<System.DateTime> LastMedicalCheckDate
		{
			get
			{
				return this._LastMedicalCheckDate;
			}
			set
			{
				if ((this._LastMedicalCheckDate != value))
				{
					this.OnLastMedicalCheckDateChanging(value);
					this.SendPropertyChanging();
					this._LastMedicalCheckDate = value;
					this.SendPropertyChanged("LastMedicalCheckDate");
					this.OnLastMedicalCheckDateChanged();
				}
			}
		}
		
		[Column(Storage="_Balance", DbType="Float")]
		public System.Nullable<double> Balance
		{
			get
			{
				return this._Balance;
			}
			set
			{
				if ((this._Balance != value))
				{
					this.OnBalanceChanging(value);
					this.SendPropertyChanging();
					this._Balance = value;
					this.SendPropertyChanged("Balance");
					this.OnBalanceChanged();
				}
			}
		}
		
		[Association(Name="Company_Employee", Storage="_Company", ThisKey="CompanyId", OtherKey="CompanyId", IsForeignKey=true)]
		public Company Company
		{
			get
			{
				return this._Company.Entity;
			}
			set
			{
				Company previousValue = this._Company.Entity;
				if (((previousValue != value) 
							|| (this._Company.HasLoadedOrAssignedValue == false)))
				{
					this.SendPropertyChanging();
					if ((previousValue != null))
					{
						this._Company.Entity = null;
						previousValue.Employees.Remove(this);
					}
					this._Company.Entity = value;
					if ((value != null))
					{
						value.Employees.Add(this);
						this._CompanyId = value.CompanyId;
					}
					else
					{
						this._CompanyId = default(string);
					}
					this.SendPropertyChanged("Company");
				}
			}
		}
		
		public event PropertyChangingEventHandler PropertyChanging;
		
		public event PropertyChangedEventHandler PropertyChanged;
		
		protected virtual void SendPropertyChanging()
		{
			if ((this.PropertyChanging != null))
			{
				this.PropertyChanging(this, emptyChangingEventArgs);
			}
		}
		
		protected virtual void SendPropertyChanged(String propertyName)
		{
			if ((this.PropertyChanged != null))
			{
				this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}
	}
	
	[Table(Name="dbo.ClockHistory")]
	public partial class ClockHistory
	{
		
		private int _BadgeHolderNumber;
		
		private string _DeviceId;
		
		private System.DateTime _TimeStamp;
		
		private short _ResponseCode;
		
		private char _AccessGranted;
		
		private string _AccompaniedBadge;
		
		public ClockHistory()
		{
		}
		
		[Column(Storage="_BadgeHolderNumber", DbType="Int NOT NULL")]
		public int BadgeHolderNumber
		{
			get
			{
				return this._BadgeHolderNumber;
			}
			set
			{
				if ((this._BadgeHolderNumber != value))
				{
					this._BadgeHolderNumber = value;
				}
			}
		}
		
		[Column(Storage="_DeviceId", DbType="Char(6) NOT NULL", CanBeNull=false)]
		public string DeviceId
		{
			get
			{
				return this._DeviceId;
			}
			set
			{
				if ((this._DeviceId != value))
				{
					this._DeviceId = value;
				}
			}
		}
		
		[Column(Storage="_TimeStamp", DbType="DateTime NOT NULL")]
		public System.DateTime TimeStamp
		{
			get
			{
				return this._TimeStamp;
			}
			set
			{
				if ((this._TimeStamp != value))
				{
					this._TimeStamp = value;
				}
			}
		}
		
		[Column(Storage="_ResponseCode", DbType="SmallInt NOT NULL")]
		public short ResponseCode
		{
			get
			{
				return this._ResponseCode;
			}
			set
			{
				if ((this._ResponseCode != value))
				{
					this._ResponseCode = value;
				}
			}
		}
		
		[Column(Storage="_AccessGranted", DbType="Char(1) NOT NULL")]
		public char AccessGranted
		{
			get
			{
				return this._AccessGranted;
			}
			set
			{
				if ((this._AccessGranted != value))
				{
					this._AccessGranted = value;
				}
			}
		}
		
		[Column(Storage="_AccompaniedBadge", DbType="VarChar(15)")]
		public string AccompaniedBadge
		{
			get
			{
				return this._AccompaniedBadge;
			}
			set
			{
				if ((this._AccompaniedBadge != value))
				{
					this._AccompaniedBadge = value;
				}
			}
		}
	}
	
	[Table(Name="dbo.Reader")]
	public partial class Reader : INotifyPropertyChanging, INotifyPropertyChanged
	{
		
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
		private string _DeviceId;
		
		private string _ReaderTypeId;
		
		private string _ReaderGroupId;
		
		private string _ToZone;
		
		private char _AllowOffline;
		
		private char _Controlled;
		
		private short _ControlledTime;
		
		private char _ClearRandomParade;
		
		private string _MealTypeId;
		
		private char _Emergency;
		
		private string _PromVersion;
		
		private char _RFProtected;
		
		private System.Nullable<char> _Accompanied;
		
		private string _AccompaniedDeviceId;
		
		private string _EquipmentTypeID;
		
		private string _BiometricsLinkType;
		
    #region Extensibility Method Definitions
    partial void OnLoaded();
    partial void OnValidate(System.Data.Linq.ChangeAction action);
    partial void OnCreated();
    partial void OnDeviceIdChanging(string value);
    partial void OnDeviceIdChanged();
    partial void OnReaderTypeIdChanging(string value);
    partial void OnReaderTypeIdChanged();
    partial void OnReaderGroupIdChanging(string value);
    partial void OnReaderGroupIdChanged();
    partial void OnToZoneChanging(string value);
    partial void OnToZoneChanged();
    partial void OnAllowOfflineChanging(char value);
    partial void OnAllowOfflineChanged();
    partial void OnControlledChanging(char value);
    partial void OnControlledChanged();
    partial void OnControlledTimeChanging(short value);
    partial void OnControlledTimeChanged();
    partial void OnClearRandomParadeChanging(char value);
    partial void OnClearRandomParadeChanged();
    partial void OnMealTypeIdChanging(string value);
    partial void OnMealTypeIdChanged();
    partial void OnEmergencyChanging(char value);
    partial void OnEmergencyChanged();
    partial void OnPromVersionChanging(string value);
    partial void OnPromVersionChanged();
    partial void OnRFProtectedChanging(char value);
    partial void OnRFProtectedChanged();
    partial void OnAccompaniedChanging(System.Nullable<char> value);
    partial void OnAccompaniedChanged();
    partial void OnAccompaniedDeviceIdChanging(string value);
    partial void OnAccompaniedDeviceIdChanged();
    partial void OnEquipmentTypeIDChanging(string value);
    partial void OnEquipmentTypeIDChanged();
    partial void OnBiometricsLinkTypeChanging(string value);
    partial void OnBiometricsLinkTypeChanged();
    #endregion
		
		public Reader()
		{
			OnCreated();
		}
		
		[Column(Storage="_DeviceId", DbType="Char(6) NOT NULL", CanBeNull=false, IsPrimaryKey=true)]
		public string DeviceId
		{
			get
			{
				return this._DeviceId;
			}
			set
			{
				if ((this._DeviceId != value))
				{
					this.OnDeviceIdChanging(value);
					this.SendPropertyChanging();
					this._DeviceId = value;
					this.SendPropertyChanged("DeviceId");
					this.OnDeviceIdChanged();
				}
			}
		}
		
		[Column(Storage="_ReaderTypeId", DbType="Char(6) NOT NULL", CanBeNull=false)]
		public string ReaderTypeId
		{
			get
			{
				return this._ReaderTypeId;
			}
			set
			{
				if ((this._ReaderTypeId != value))
				{
					this.OnReaderTypeIdChanging(value);
					this.SendPropertyChanging();
					this._ReaderTypeId = value;
					this.SendPropertyChanged("ReaderTypeId");
					this.OnReaderTypeIdChanged();
				}
			}
		}
		
		[Column(Storage="_ReaderGroupId", DbType="Char(6) NOT NULL", CanBeNull=false)]
		public string ReaderGroupId
		{
			get
			{
				return this._ReaderGroupId;
			}
			set
			{
				if ((this._ReaderGroupId != value))
				{
					this.OnReaderGroupIdChanging(value);
					this.SendPropertyChanging();
					this._ReaderGroupId = value;
					this.SendPropertyChanged("ReaderGroupId");
					this.OnReaderGroupIdChanged();
				}
			}
		}
		
		[Column(Storage="_ToZone", DbType="Char(6) NOT NULL", CanBeNull=false)]
		public string ToZone
		{
			get
			{
				return this._ToZone;
			}
			set
			{
				if ((this._ToZone != value))
				{
					this.OnToZoneChanging(value);
					this.SendPropertyChanging();
					this._ToZone = value;
					this.SendPropertyChanged("ToZone");
					this.OnToZoneChanged();
				}
			}
		}
		
		[Column(Storage="_AllowOffline", DbType="Char(1) NOT NULL")]
		public char AllowOffline
		{
			get
			{
				return this._AllowOffline;
			}
			set
			{
				if ((this._AllowOffline != value))
				{
					this.OnAllowOfflineChanging(value);
					this.SendPropertyChanging();
					this._AllowOffline = value;
					this.SendPropertyChanged("AllowOffline");
					this.OnAllowOfflineChanged();
				}
			}
		}
		
		[Column(Storage="_Controlled", DbType="Char(1) NOT NULL")]
		public char Controlled
		{
			get
			{
				return this._Controlled;
			}
			set
			{
				if ((this._Controlled != value))
				{
					this.OnControlledChanging(value);
					this.SendPropertyChanging();
					this._Controlled = value;
					this.SendPropertyChanged("Controlled");
					this.OnControlledChanged();
				}
			}
		}
		
		[Column(Storage="_ControlledTime", DbType="SmallInt NOT NULL")]
		public short ControlledTime
		{
			get
			{
				return this._ControlledTime;
			}
			set
			{
				if ((this._ControlledTime != value))
				{
					this.OnControlledTimeChanging(value);
					this.SendPropertyChanging();
					this._ControlledTime = value;
					this.SendPropertyChanged("ControlledTime");
					this.OnControlledTimeChanged();
				}
			}
		}
		
		[Column(Storage="_ClearRandomParade", DbType="Char(1) NOT NULL")]
		public char ClearRandomParade
		{
			get
			{
				return this._ClearRandomParade;
			}
			set
			{
				if ((this._ClearRandomParade != value))
				{
					this.OnClearRandomParadeChanging(value);
					this.SendPropertyChanging();
					this._ClearRandomParade = value;
					this.SendPropertyChanged("ClearRandomParade");
					this.OnClearRandomParadeChanged();
				}
			}
		}
		
		[Column(Storage="_MealTypeId", DbType="Char(6)")]
		public string MealTypeId
		{
			get
			{
				return this._MealTypeId;
			}
			set
			{
				if ((this._MealTypeId != value))
				{
					this.OnMealTypeIdChanging(value);
					this.SendPropertyChanging();
					this._MealTypeId = value;
					this.SendPropertyChanged("MealTypeId");
					this.OnMealTypeIdChanged();
				}
			}
		}
		
		[Column(Storage="_Emergency", DbType="Char(1) NOT NULL")]
		public char Emergency
		{
			get
			{
				return this._Emergency;
			}
			set
			{
				if ((this._Emergency != value))
				{
					this.OnEmergencyChanging(value);
					this.SendPropertyChanging();
					this._Emergency = value;
					this.SendPropertyChanged("Emergency");
					this.OnEmergencyChanged();
				}
			}
		}
		
		[Column(Storage="_PromVersion", DbType="VarChar(30) NOT NULL", CanBeNull=false)]
		public string PromVersion
		{
			get
			{
				return this._PromVersion;
			}
			set
			{
				if ((this._PromVersion != value))
				{
					this.OnPromVersionChanging(value);
					this.SendPropertyChanging();
					this._PromVersion = value;
					this.SendPropertyChanged("PromVersion");
					this.OnPromVersionChanged();
				}
			}
		}
		
		[Column(Storage="_RFProtected", DbType="Char(1) NOT NULL")]
		public char RFProtected
		{
			get
			{
				return this._RFProtected;
			}
			set
			{
				if ((this._RFProtected != value))
				{
					this.OnRFProtectedChanging(value);
					this.SendPropertyChanging();
					this._RFProtected = value;
					this.SendPropertyChanged("RFProtected");
					this.OnRFProtectedChanged();
				}
			}
		}
		
		[Column(Storage="_Accompanied", DbType="Char(1)")]
		public System.Nullable<char> Accompanied
		{
			get
			{
				return this._Accompanied;
			}
			set
			{
				if ((this._Accompanied != value))
				{
					this.OnAccompaniedChanging(value);
					this.SendPropertyChanging();
					this._Accompanied = value;
					this.SendPropertyChanged("Accompanied");
					this.OnAccompaniedChanged();
				}
			}
		}
		
		[Column(Storage="_AccompaniedDeviceId", DbType="Char(6)")]
		public string AccompaniedDeviceId
		{
			get
			{
				return this._AccompaniedDeviceId;
			}
			set
			{
				if ((this._AccompaniedDeviceId != value))
				{
					this.OnAccompaniedDeviceIdChanging(value);
					this.SendPropertyChanging();
					this._AccompaniedDeviceId = value;
					this.SendPropertyChanged("AccompaniedDeviceId");
					this.OnAccompaniedDeviceIdChanged();
				}
			}
		}
		
		[Column(Storage="_EquipmentTypeID", DbType="Char(12)")]
		public string EquipmentTypeID
		{
			get
			{
				return this._EquipmentTypeID;
			}
			set
			{
				if ((this._EquipmentTypeID != value))
				{
					this.OnEquipmentTypeIDChanging(value);
					this.SendPropertyChanging();
					this._EquipmentTypeID = value;
					this.SendPropertyChanged("EquipmentTypeID");
					this.OnEquipmentTypeIDChanged();
				}
			}
		}
		
		[Column(Storage="_BiometricsLinkType", DbType="VarChar(15)")]
		public string BiometricsLinkType
		{
			get
			{
				return this._BiometricsLinkType;
			}
			set
			{
				if ((this._BiometricsLinkType != value))
				{
					this.OnBiometricsLinkTypeChanging(value);
					this.SendPropertyChanging();
					this._BiometricsLinkType = value;
					this.SendPropertyChanged("BiometricsLinkType");
					this.OnBiometricsLinkTypeChanged();
				}
			}
		}
		
		public event PropertyChangingEventHandler PropertyChanging;
		
		public event PropertyChangedEventHandler PropertyChanged;
		
		protected virtual void SendPropertyChanging()
		{
			if ((this.PropertyChanging != null))
			{
				this.PropertyChanging(this, emptyChangingEventArgs);
			}
		}
		
		protected virtual void SendPropertyChanged(String propertyName)
		{
			if ((this.PropertyChanged != null))
			{
				this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}
	}
}
#pragma warning restore 1591