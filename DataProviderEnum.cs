﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataProviderFactory
{
	enum DataProviderEnum
	{
		SqlServer,
#if PC
		OleDb,
#endif
		Odbc,

	}
}
